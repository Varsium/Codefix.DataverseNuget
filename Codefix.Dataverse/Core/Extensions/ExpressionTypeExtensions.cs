using Codefix.Dataverse.Core.Conventions.Constants;
using System.Linq.Expressions;

namespace Codefix.Dataverse.Core.Extensions
{
    internal static class ExpressionTypeExtensions
    {
        public static string ToODataOperator(this ExpressionType expressionType) => expressionType switch
        {
            ExpressionType.And => ODataLogicalOperations.And,
            ExpressionType.AndAlso => ODataLogicalOperations.And,
            ExpressionType.Or => ODataLogicalOperations.Or,
            ExpressionType.OrElse => ODataLogicalOperations.Or,
            ExpressionType.Equal => ODataLogicalOperations.Equal,
            ExpressionType.Not => ODataLogicalOperations.Not,
            ExpressionType.NotEqual => ODataLogicalOperations.NotEqual,
            ExpressionType.LessThan => ODataLogicalOperations.LessThan,
            ExpressionType.LessThanOrEqual => ODataLogicalOperations.LessThanOrEqual,
            ExpressionType.GreaterThan => ODataLogicalOperations.GreaterThan,
            ExpressionType.GreaterThanOrEqual => ODataLogicalOperations.GreaterThanOrEqual,
            _ => default,
        };
    }
}
