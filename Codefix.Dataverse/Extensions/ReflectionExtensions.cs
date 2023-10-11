using Codefix.Dataverse.Attributes;
using Codefix.Dataverse.Entities;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Codefix.Dataverse.Extensions
{
#pragma warning disable CS8600
#pragma warning disable CS8603
#pragma warning disable CS8602
    public static class ReflectionExtensions
    {
        private const string OdataBinder = "@odata.bind";
        public static object MapToOdataClass(this object customClass, string method)
        {
            var members = customClass.GetType().GetProperties().ToList();
            return ReturnMemberDictionary(members, customClass, method);
        }

        private static Dictionary<string, object> ReturnMemberDictionary(List<PropertyInfo> members, object customClass, string? method)
        {
            if (customClass.GetType() == typeof(JObject))
            {
                var serializer = JsonSerializer.CreateDefault();
                using StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
                serializer.Serialize(sw, customClass);
                using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
                {
                    using (TextReader sr = new StringReader(sw.ToString()))
                    using (JsonReader reader = new JsonTextReader(sr))
                    {

                        var jDict = serializer.Deserialize<Dictionary<string, object>>(reader);
                        return jDict.DeleteDictionaryNonBindingIds();
                    }

                }


            }
            var dict = new Dictionary<string, object>();
            foreach (var member in members)
            {
                if (member.CheckIsReadOnly().GetValueOrDefault(false))
                {
                    continue;
                }
                var property = member.GetPropertyName();
                var bindingProperty = member.GetBindValue();
                if (!string.IsNullOrEmpty(property))
                {
                    CreateProperty(method, property, member, customClass, ref dict);
                }
                CreateBindings(property, bindingProperty, member, customClass, ref dict);
            }
            return dict.DeleteDictionaryNonBindingIds();
        }
        public static Dictionary<string, object> DeleteDictionaryNonBindingIds(this Dictionary<string, object> content)
        {
            var copy = content;
            foreach (var key in copy.Keys)
            {
                if (key.Length > "_value".Length)
                {
                    if (key.Substring(key.Length - "_value".Length).Contains("_value") && key.StartsWith('_'))
                    {
                        content.Remove(key);
                    };
                }
            }
            return content;
        }
        public static string GetTableName(this Type type)
        {

            var tableNameAttribute = (ODataTableAttribute)Attribute.GetCustomAttribute(type, typeof(ODataTableAttribute));

            if (tableNameAttribute != null)
            {

                return tableNameAttribute.Name;

            }

            return type.Name;
        }

        public static string GetTableName<T>(this T tEntity) where T : class
        {

            var tableNameAttribute = (ODataTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ODataTableAttribute));

            if (tableNameAttribute != null)
            {

                return tableNameAttribute.Name;

            }

            return typeof(T).Name;
        }
        public static string? GetPropertyName(this MemberInfo info)
        {
            var property = (JsonPropertyAttribute)Attribute.GetCustomAttribute(info, typeof(JsonPropertyAttribute));
            if (property != null)
            { return property.PropertyName; }
            var propertyName = (JsonPropertyNameAttribute)Attribute.GetCustomAttribute(info, typeof(JsonPropertyNameAttribute));
            if (propertyName != null)
            {
                return propertyName.Name;
            }
            var logicalName = (AttributeLogicalNameAttribute)Attribute.GetCustomAttribute(info, typeof(AttributeLogicalNameAttribute));
            if (logicalName != null)
            {
                return logicalName.LogicalName;
            }
            var relationName = (RelationshipSchemaNameAttribute)Attribute.GetCustomAttribute(info, typeof(RelationshipSchemaNameAttribute));
            if (relationName != null)
            {
                return relationName.SchemaName;
            }
            return info.Name;
        }
        public static bool? CheckIsReadOnly(this MemberInfo info)
        {
            var property = (ODataReadonlyAttribute)Attribute.GetCustomAttribute(info, typeof(ODataReadonlyAttribute));
            if (property != null)
            { return property.Readonly; }

            return false;
        }
        public static bool CheckIsPrimaryKey
            (this MemberInfo info)
        {
            var property = (OdataPrimaryKeyAttribute)Attribute.GetCustomAttribute(info, typeof(OdataPrimaryKeyAttribute));
            if (property != null)
            { return true; }

            return false;
        }
        public static object? GetValue(this object customClass, MemberInfo prop)
        {
            switch (prop.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)prop).GetValue(customClass);
                case MemberTypes.Property:
                    return ((PropertyInfo)prop).GetValue(customClass);
                default:
                    throw new NotImplementedException();
            }
            var value = customClass.GetType()?.GetProperty(prop.Name)?.GetValue(customClass);
            return value;
        }
        public static object? GetFirstObjectValue(this object customClass)
        {
            var value = customClass.GetValue(customClass.GetType()?.GetMembers().FirstOrDefault(m => m.MemberType == MemberTypes.Field));
            return value;
        }
        public static ODataBindAttribute? GetBindValue(this MemberInfo member)
        {
            var tableNameAttribute = (ODataBindAttribute)Attribute.GetCustomAttribute(member, typeof(ODataBindAttribute));

            if (tableNameAttribute != null)
            {

                return tableNameAttribute;

            }

            return null;
        }
        public static bool AddIgnoreMember(this MemberInfo member, object customClass)
        {
            var jsonObjectAttribute = (JsonObjectAttribute)customClass.GetType().GetCustomAttribute(typeof(JsonObjectAttribute));
            var property = (JsonPropertyAttribute)Attribute.GetCustomAttribute(member, typeof(JsonPropertyAttribute));
            if (jsonObjectAttribute?.ItemNullValueHandling == NullValueHandling.Ignore)
            {
                if (customClass.GetValue(member) is not null)
                {
                    return true;
                }
                return false;
            }
            if (property?.NullValueHandling == NullValueHandling.Ignore)
            {
                return false;
            }
            return true;
        }
        private static bool IsValueProp(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                return false;
            }
            if (property.Length <= "_value".Length)
            {
                return false;
            }

            return (property?.EndsWith("_value")).GetValueOrDefault(false) && (property?.StartsWith('_')).GetValueOrDefault(false);
        }
        private static bool ConditionAccepted(object customClass, MemberInfo member)
        {
            var memberType = customClass.GetValue(member)?.GetType();
            if ((memberType?.IsGenericType).GetValueOrDefault(false))
            {
                return false;
            }
            if (!(memberType?.IsClass).GetValueOrDefault(false)
                && (!memberType?.FullName?.ToLower()?.Contains("system.")).GetValueOrDefault(false))
            {
                return true;
            }
            if ((memberType?.GetType()?.IsClass).GetValueOrDefault(false)
                 && (memberType?.FullName?.ToLower()?.Contains("system.")).GetValueOrDefault(false))
            {
                return true;
            }
            return false;
        }
        private static void CreateChildRecords(string method, object customClass, MemberInfo member, string property, ref Dictionary<string, object> dict)
        {
            if (method?.ToUpper() == HttpMethod.Post.Method.ToUpper())
            {
                var subObject = customClass.GetValue(member);
                if (member.AddIgnoreMember(customClass) && subObject is not null)
                {
                    var memberType = subObject.GetType();
                    if ((memberType?.IsGenericType).GetValueOrDefault(false))
                    {
                        var dictList = new List<object>();
                        foreach (var obj in subObject as IList)
                        {
                            dictList.Add(ReturnMemberDictionary(obj.GetType().GetProperties().ToList(), obj, method));
                        }
                        dict.TryAdd(property, dictList);
                        return;
                    }
                    dict.Add(property,/* new List<object>() {*/ ReturnMemberDictionary(subObject.GetType().GetProperties().ToList(), subObject, method) /*}*/);
                }
            }
        }
        private static void CreateBindings(string property, ODataBindAttribute bindingProperty, PropertyInfo member, object customClass, ref Dictionary<string, object> dict)
        {
            if (IsValueProp(property) && bindingProperty is not null && Guid.TryParse(member?.GetValue(customClass)?.ToString(), out var guid))
            {
                if (guid.ToString() != new Guid().ToString())
                {
                    var bindName = bindingProperty.Name;
                    bindName = !bindName.Contains(OdataBinder) ? bindName + OdataBinder : bindName;
                    dict.TryAdd(bindName, $"/{bindingProperty.ReferenceTable}({guid})");
                }
            };
        }
        private static void CreateProperty(string method, string property, MemberInfo member, object customClass, ref Dictionary<string, object> dict)
        {
            if (ConditionAccepted(customClass, member))
            {
                if (customClass.GetValue(member) is not null)
                {
                    dict.TryAdd(property, customClass.GetValue(member));
                }
                if (customClass.GetValue(member) is null && member.AddIgnoreMember(customClass))
                {
                    dict.TryAdd(property, null);
                }
                return;
            }
            CreateChildRecords(method, customClass, member, property, ref dict);
        }
        internal static string GetPrimaryKey(this object customClass)
        {
            var primaryId = string.Empty;
            var members = customClass.GetType().GetProperties().ToList();
            foreach (var member in members)
            {
                if (member.CheckIsPrimaryKey() && string.IsNullOrEmpty(primaryId))
                {
                    primaryId = (string)member.GetValue(customClass);
                }
            }
            return primaryId;
        }

        internal static KeyValuePair<string, string>? CheckOboFlow(this object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            bool callerObjectIdFilled = false;
            bool mscrmCallerIdFilled = false;

            var match = properties.Where(p => Attribute.IsDefined(p, typeof(ODataReadonlyAttribute)) && (p.Name == nameof(DataverseRecord.OnBehalfOfAadId) || p.Name == nameof(DataverseRecord.OnBehalfOfMSCRMId)) && p.GetValue(obj) is not null).ToList();
            if (match?.Count >= 2)
            {
                throw new InvalidOperationException("Both CallerObjectId and MSCRMCallerID are filled in.");
            }

            if (match is null || !(match?.Any()).GetValueOrDefault(false))
            {
                return null;
            }
            return new KeyValuePair<string, string>(match!.First().GetPropertyName(), match!.First().GetValue(obj) as string);
        }

    }



#pragma warning restore CS8603
#pragma warning restore CS8600
#pragma warning restore CS8602
}
