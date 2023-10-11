using System;
using System.Reflection;

namespace Codefix.Dataverse.Core.Extensions
{
    internal static class ReflectionExtensions
    {
        public static object GetValue(this MemberInfo memberInfo, object obj = default) => memberInfo switch
        {
            FieldInfo fieldInfo => fieldInfo.GetValue(obj),
            PropertyInfo propertyInfo => propertyInfo.GetValue(obj, default),
            _ => default,
        };

        public static bool IsNullableType(this Type type) =>
            Nullable.GetUnderlyingType(type) != default;
    }
}
