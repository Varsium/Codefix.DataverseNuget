using Azure.Core;
using Codefix.Dataverse.Authentication;
using Codefix.Dataverse.Configs;
using Codefix.Dataverse.Extensions;
using Codefix.Dataverse.Models.Dataverse;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Text;

namespace Codefix.Dataverse.Services
{
    public class DataverseService : IDataverseService
    {
        private readonly DataverseConfig _config;
        private readonly HttpClient _httpClient;
        private readonly DataverseAuthConfig _authProvider;
        private readonly ILogger<DataverseService> _logger;
        private readonly TokenRequestContext _tokenRequest;
        private readonly List<string> _filterList = new List<string>() { "?$expand", "?$filter", "?$orderby", "?$select", "?$top", "?$skip", "?$count", "?$search" };

        public DataverseService(string baseUri, TokenCredential credential, ILogger<DataverseService> log)
        {
            _config = new DataverseConfig(baseUri, credential);
        }

        public DataverseService(DataverseAuthConfig authProvider, ILogger<DataverseService> log)
        {
            _logger = log;
            _authProvider = authProvider;
            _config = authProvider.GetConfig();
            _httpClient = new HttpClient(
                  new HttpClientHandler()
                  {
                      UseCookies = false,
                  })
            {
                BaseAddress = new Uri((_config?.BaseUrl) ?? "https://nobaseurl.com"),
                Timeout = new TimeSpan(0, 2, 0),
            };
            _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", _config?.ApiODataVersion);
            _httpClient.DefaultRequestHeaders.Add("OData-Version", _config?.ApiODataVersion);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.DefaultConnectionLimit = 65000;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = false;
        }

        public async Task<T> ConfigureDataverseRequestAsync<T>(HttpMethod httpMethod, string table, string? filter = null, object? customClass = null) where T : class
        {

            var content = string.Empty;
            if (customClass != null)
            {
                content = JsonConvert.SerializeObject(customClass.MapToOdataClass(httpMethod.Method.ToString()));
            }

            var request = new DataverseRequest
            {
                HttpMethod = httpMethod,
                Headers = new Dictionary<string, string>()
                {
                    { "Accept", "application/json" },
                    { "Prefer", "return=representation" },
               //     {"Prefer","odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"" }
                },
                ApiRequest = $"{table}",
                Content = content,
            };

            var oboFlow = customClass?.CheckOboFlow();
            if (oboFlow is not null)
            {
                request.Headers.TryAdd(oboFlow.Value.Key, oboFlow.Value.Value);
            }
            request.ApiRequest = CheckFilterConditionsAndReturnApiRequest(filter, request.ApiRequest);
            return await GetAllRecords<T>(request);

        }
        public async Task<T> PostEntityInDataverse<T>(string table, T objectToPost)
            where T : class
        {
            var result = await ConfigureDataverseRequestAsync<T>(HttpMethod.Post, table, null, objectToPost);
            return result;
        }
        public async Task<T> PatchEntityInDataverse<T>(string table, T objectToPost)
            where T : class
        {
            if (!table.IsAsPatch() && string.IsNullOrEmpty(objectToPost.GetPrimaryKey()))
            {
                throw new Exception("You should patch an entity with tablename(Guid), Use the OdataPrimarykey attribute of manually fil this in.");
            }

            table = Createdpatchstring(table, objectToPost);
            var result = await ConfigureDataverseRequestAsync<T>(HttpMethod.Patch, table, null, objectToPost);
            return result;
        }
        public async Task<T> PutEntityInDataverse<T>(string table, T objectToPost)
        where T : class
        {
            var result = await ConfigureDataverseRequestAsync<T>(HttpMethod.Put, table, null, objectToPost);
            return result;
        }

        public async Task<T> PostEntityInDataverse<T>(string table, object objectToPost)
           where T : class
        {
            var result = await ConfigureDataverseRequestAsync<T>(HttpMethod.Post, table, null, objectToPost);
            return result;
        }
        public async Task<T> PatchEntityInDataverse<T>(string table, object objectToPost)
            where T : class
        {
            if (!table.IsAsPatch() && string.IsNullOrEmpty(objectToPost.GetPrimaryKey()))
            {
                throw new Exception("You should patch an entity with tablename(Guid), Use the OdataPrimarykey attribute of manually fil this in.");
            }

            table = Createdpatchstring(table, objectToPost);
            var result = await ConfigureDataverseRequestAsync<T>(HttpMethod.Patch, table, null, objectToPost);
            return result;
        }
        public async Task<T> PutEntityInDataverse<T>(string table, object objectToPost)
        where T : class
        {
            var result = await ConfigureDataverseRequestAsync<T>(HttpMethod.Put, table, null, objectToPost);
            return result;
        }
        public async Task<T> GetEntityInDataverse<T>(string table, string? filter = null)
          where T : class
        {
            var result = await ConfigureDataverseRequestAsync<T>(HttpMethod.Get, table, filter);
            return result;
        }
        public async Task<bool> DeleteEntityInDataverse<T>(string tableNameWithGuid) where T : class
        {
            try
            {
                if (tableNameWithGuid.Contains("(") && tableNameWithGuid.Contains(")") && tableNameWithGuid.IndexOf(")") - tableNameWithGuid.IndexOf("(") > 1)
                {
                    var (body, status) = await GetBytes(new DataverseRequest
                    {
                        HttpMethod = HttpMethod.Delete,
                        Headers = new Dictionary<string, string>()
                             {
                                 { "Accept", "application/json" },
                                 { "Prefer", "return=representation" },
                             },
                        ApiRequest = tableNameWithGuid,
                    }, false);
                    if (!status.IsSuccessStatusCode())
                    {
                        return false;
                    }
                    return true;
                }
                throw new Exception("you have to give an ID to the Delete request.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return false;
            }
        }

        internal string CheckFilterConditionsAndReturnApiRequest(string? filter, string apiRequest)
        {

            if (filter == string.Empty || filter == null)
            {
                return apiRequest;
            }
            var operatorResult = "?";

            if (apiRequest.Contains("?$"))
            {
                operatorResult = "&";
            }
            var boolResult = new List<bool>();

            if (_filterList.Any(key => key == filter[..key.Length]))
            {
                apiRequest += filter;
                return apiRequest;
            }
            apiRequest += $"{operatorResult}{filter}";

            if (apiRequest.EndsWith("&") || apiRequest.EndsWith("?"))
            {
                apiRequest = apiRequest.Remove(apiRequest.Length - 1);
            }
            return apiRequest;
        }
        internal async Task<T> GetAllRecords<T>(DataverseRequest request) where T : class
        {
            var mainObject = await ExecuteDataverseGetRequestAsync<T>(request);
            while (mainObject.OdataNextLink != null)
            {
                var temporaryRequest = request;
                temporaryRequest.ApiRequest = mainObject.OdataNextLink.AbsoluteUri;
                temporaryRequest.Headers = null;
                temporaryRequest.Content = null;

                if (typeof(T).IsGenericType)
                {
                    var result = await ExecuteDataverseGetRequestAsync<T>(temporaryRequest, true);
                    if (result.Value is IEnumerable<object> resultObjects && mainObject.Value is IEnumerable<object> mainObjects)
                    {
                        var serializer = JsonSerializer.CreateDefault();

                        var mainList = mainObjects.ToList();
                        mainList.AddRange(resultObjects);
                        using StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
                        serializer.Serialize(sw, mainList);

                        using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
                        {
                            using (TextReader sr = new StringReader(sw.ToString()))
                            using (JsonReader reader = new JsonTextReader(sr))
                            {
                                mainObject.Value = serializer.Deserialize<T>(reader);
                            }

                        }
                        mainObject.OdataNextLink = result.OdataNextLink;
                    };

                }
            }
            return mainObject.Value;
        }
        internal async Task<DataverseGetResponse<T>> ExecuteDataverseGetRequestAsync<T>(DataverseRequest request, bool nextRows = false) where T : class
        {
            var counter = 0;
            var (body, statusCode) = await GetBytes(request, nextRows);
            while (statusCode == HttpStatusCode.TooManyRequests && counter <= 40)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                (body, statusCode) = await GetBytes(request, nextRows);
            }
            if (!statusCode.IsSuccessStatusCode())
            {

                throw new Exception($"Failed to call the Dataverse-service: request '{request.ApiRequest}' with HTTP-method '{request.HttpMethod}' returned HTTP-status {statusCode} with reason : {Encoding.UTF8.GetString(body)}");
            }

            _logger?.LogInformation("Successfully called the Dataverse-service: request '{RequestUri}' with HTTP-method '{Method}' returned HTTP-status {statusCode}", request.ApiRequest, request.HttpMethod, statusCode);

            if (statusCode == HttpStatusCode.NoContent)
            {
                return null;
            }
            return await ConvertDataverseGetResponse<T>(body);
        }

        internal async Task<(byte[] body, HttpStatusCode statusCode)> GetBytes(DataverseRequest request, bool nextRows)
        {

            var httpRequest = await CreateHttpRequest(request, nextRows);

            using var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead);
            var stream = await response.Content.ReadAsStreamAsync(new CancellationToken());


            using var memStream = new MemoryStream(0);
            stream.CopyTo(memStream);
            stream.Flush();
            stream.Close();
            return (memStream.ToArray(), response.StatusCode);
        }
        internal async Task<HttpRequestMessage> CreateHttpRequest(DataverseRequest request, bool nextRows)
        {
            var requestUri = new Uri($"{_config.BaseUrl}{_config.ApiUrl}{request.ApiRequest}");
            if (nextRows)
            {
                requestUri = new Uri(request.ApiRequest, UriKind.Absolute);
            }
            HttpRequestMessage httpRequest = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = request.HttpMethod,
                Headers =
                    {
                        { HttpRequestHeader.Authorization.ToString(), await _authProvider.GetAccessTokenAsync() },

                    },
            };

            if (request.Headers != null)
            {
                foreach (KeyValuePair<string, string> header in request.Headers)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }
            }

            if (request.Content != null)
            {

                httpRequest.Content = new StringContent(
                    request.Content,
                    Encoding.UTF8,
                    "application/json");
            }
            return httpRequest;
        }
        internal async Task<DataverseGetResponse<T>> ConvertDataverseGetResponse<T>(byte[] response) where T : class
        {
            if (typeof(T).IsGenericType)
            {
                var result = Deserialize<DataverseGetResponse<T>>(response);
                return result;
            }
            var responseWithList = Deserialize<DataverseGetResponse<IList<T>>>(response);

            if (responseWithList.Value != null)
            {
                return new DataverseGetResponse<T>()
                {
                    OdataContext = responseWithList.OdataContext,
                    OdataNextLink = responseWithList.OdataNextLink,
                    Value = responseWithList.Value?.FirstOrDefault()
                };
            }
            var classAsResponse = Deserialize<T>(response);
            return new DataverseGetResponse<T>()
            {
                OdataContext = responseWithList.OdataContext,
                OdataNextLink = responseWithList.OdataNextLink,
                Value = classAsResponse,
            };

        }
        internal T Deserialize<T>(byte[] response) where T : class
        {
            using var responseStream = new MemoryStream(response);
            using (StreamReader sr = new StreamReader(responseStream))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    reader.SupportMultipleContent = true;
                    JsonSerializer serializer = new JsonSerializer();
                    var result = serializer.Deserialize<T>(reader);

                    return result;
                }
            }
        }
        internal string Createdpatchstring<T>(string table, T customClass) where T : class
        {
            if (!table.IsAsPatch() && !string.IsNullOrEmpty(customClass.GetPrimaryKey()))
            {
                var tableName = customClass.GetType().GetTableName();
                var pK = customClass.GetPrimaryKey();
                if (table.Length < tableName.Length + pK.Length)
                    table = $"{tableName}({pK})";
                return table;
            }
            return table;
        }

        public async Task<int> CountAsync(string table, string tracedQuery)
        {
            var result = await ConfigureDataverseRequestAsync<dynamic>(HttpMethod.Get, table, tracedQuery);
            return result.Count;
        }
        public async Task<int> CountAsync<T>(string tracedQuery) where T : class
        {
            var request = new DataverseRequest
            {
                HttpMethod = HttpMethod.Get,
                Headers = new Dictionary<string, string>()
                {
                    { "Accept", "application/json" },
                    { "Prefer", "return=representation" },
                    //     {"Prefer","odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"" }
                },
                ApiRequest = $"{typeof(T).GetTableName()}{tracedQuery}",
            };
            var bytes = await GetBytes(request, false);
            if (bytes.statusCode.IsSuccessStatusCode())
            {
                return Deserialize<DataverseGetResponse<object>>(bytes.body).Count;
            }

            return 0;
        }

    }
}
