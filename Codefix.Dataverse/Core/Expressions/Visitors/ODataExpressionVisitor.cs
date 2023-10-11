﻿using Codefix.Dataverse.Core.Conventions.Constants;
using Codefix.Dataverse.Core.Conventions.Functions;
using Codefix.Dataverse.Core.Extensions;
using Codefix.Dataverse.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Codefix.Dataverse.Core.Expressions.Visitors
{
    internal class ODataExpressionVisitor
    {
        public ODataExpressionVisitor()
        {
        }

        protected string VisitExpression(LambdaExpression topExpression, Expression expression) => expression switch
        {
            BinaryExpression binaryExpression => VisitBinaryExpression(topExpression, binaryExpression),
            MemberExpression memberExpression => VisitMemberExpression(topExpression, memberExpression),
            ConstantExpression constantExpression => VisitConstantExpression(topExpression, constantExpression),
            MethodCallExpression methodCallExpression => VisitMethodCallExpression(topExpression, methodCallExpression),
            NewExpression newExpression => VisitNewExpression(topExpression, newExpression),
            UnaryExpression unaryExpression => VisitUnaryExpression(topExpression, unaryExpression),
            LambdaExpression lambdaExpression => VisitLambdaExpression(topExpression, lambdaExpression),
            ParameterExpression parameterExpression => VisitParameterExpression(topExpression, parameterExpression),
            BlockExpression blockExpression => VisitBlockExpression(topExpression, blockExpression),
            _ => default,
        };

        [ExcludeFromCodeCoverage]
        protected virtual string VisitBinaryExpression(LambdaExpression topExpression, BinaryExpression binaryExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitConstantExpression(LambdaExpression topExpression, ConstantExpression constantExpression) => default;

        protected virtual string VisitMethodCallExpression(LambdaExpression topExpression, MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.Name.ToUpper() == nameof(ODataLogicalOperations.Contains).ToUpper())
            {


                if (methodCallExpression.Object is MemberExpression propertyExpression && methodCallExpression.Arguments.Count == 1)
                {
                    string propertyName = VisitExpression(topExpression, methodCallExpression.Object);

                    var containsValue = VisitExpression(topExpression, methodCallExpression.Arguments[0]);

                    var result = nameof(ODataLogicalOperations.Contains).ToLowerInvariant() + QuerySeparators.LeftBracket + propertyName + QuerySeparators.Comma + containsValue.ToString() + QuerySeparators.RigthBracket;
                    return result;
                }

            }
            if (methodCallExpression.Method.DeclaringType == typeof(ODataProperty))
            {
                switch (methodCallExpression.Method.Name)
                {
                    case nameof(ODataProperty.FromPath):
                        string propertyPath = (string)new ValueExpression().GetValue(methodCallExpression.Arguments[0]);
                        var propertyNames = propertyPath.Split('.');

                        MemberExpression memberExpression = Expression.PropertyOrField(
                            Expression.Parameter(topExpression.Parameters[0].Type, "m"),
                            propertyNames[0]);

                        for (var index = 1; index < propertyNames.Length; index++)
                        {
                            memberExpression = Expression.PropertyOrField(memberExpression, propertyNames[index]);
                        }

                        var genericMethodType = methodCallExpression.Method.GetGenericArguments()[0];
                        if (genericMethodType != memberExpression.Type)
                            throw new ArgumentException(
                                $"The type '{genericMethodType.FullName}' specified when calling 'ODataProperty.FromPath<T>(\"{propertyPath}\")' does not match the expected type '{memberExpression.Type.FullName}' defined by the model.");

                        return VisitMemberExpression(topExpression, memberExpression);
                }
            }

            throw new NotSupportedException($"Method {methodCallExpression.Method.Name} not supported");
        }

        [ExcludeFromCodeCoverage]
        protected virtual string VisitNewExpression(LambdaExpression topExpression, NewExpression newExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitUnaryExpression(LambdaExpression topExpression, UnaryExpression unaryExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitLambdaExpression(LambdaExpression topExpression, LambdaExpression lambdaExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitParameterExpression(LambdaExpression topExpression, ParameterExpression parameterExpression) => default;

        [ExcludeFromCodeCoverage]
        protected virtual string VisitMemberExpression(LambdaExpression topExpression, MemberExpression memberExpression)
        {
            var memberName = VisitExpression(topExpression, memberExpression.Expression);

            if (string.IsNullOrEmpty(memberName))
            {

                return memberExpression.Member.GetPropertyName();
            }

            return memberExpression.Member.DeclaringType.IsNullableType() ?
                memberName
                :
                $"{memberName}/{memberExpression.Member.GetPropertyName()}";
        }
        [ExcludeFromCodeCoverage]
        protected virtual string VisitBlockExpression(LambdaExpression topExpression, BlockExpression blockExpression)
        {
            var blockresult = string.Empty;
            foreach (var expression in blockExpression.Expressions)
            {
                var idk = VisitExpression(topExpression, expression);
                blockresult += idk;

            }
            return $"({blockresult})";
        }

        public virtual string ToQuery(LambdaExpression expression) =>
            VisitExpression(expression, expression.Body);
    }
}