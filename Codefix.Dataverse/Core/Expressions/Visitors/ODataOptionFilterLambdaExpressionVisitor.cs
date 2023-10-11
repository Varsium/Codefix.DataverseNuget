using System;
using System.Linq.Expressions;
using Codefix.Dataverse.Core.Options;

namespace Codefix.Dataverse.Core.Expressions.Visitors
{
    internal class ODataOptionFilterLambdaExpressionVisitor : ODataOptionFilterExpressionVisitor
    {
        public ODataOptionFilterLambdaExpressionVisitor(ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(odataQueryBuilderOptions)
        {
        }

        protected override string VisitParameterExpression(LambdaExpression topExpression, ParameterExpression parameterExpression) =>
            parameterExpression.Name;
    }
}
