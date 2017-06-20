using SecurityUnified.Exceptions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SecurityUnified.Serialization
{
    public static class ExpressionExtentions
    {
        //public static BinaryExpression Contains<T, M, C>(M propertyExp, C someValue) where M : MemberExpression where C : ConstantExpression
        //{
        //    //var parameterExp = Expression.Parameter(typeof(T), "type");
        //    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        //    var containsMethodExp = Expression.Call(propertyExp, method, someValue);
        //    return Expression.MakeBinary(ExpressionType.OrElse, containsMethodExp, Expression.Constant(false));
        //}

        public static MemberInfo GetMemberInfo(this Type type, string fieldName)
        {
            MemberInfo memberInfo = null;

            foreach (var interfType in type.GetInterfaces())
            {
                var memberInfos = interfType.GetMember(fieldName, System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (memberInfos.Length > 0)
                {
                    memberInfo = memberInfos[0]; //CONSIDER: multiple fields can be found
                }
            }
            if (memberInfo == null)
            {
                var memberInfos = type.GetMember(fieldName, System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (memberInfos.Length > 0)
                {
                    memberInfo = memberInfos[0]; //CONSIDER: multiple fields can be found
                }
            }

            return memberInfo;
        }

        public static Expression GetFieldAccessExpression(this ParameterExpression sourceExpression, string fieldName)
        {
            System.Reflection.MemberInfo memberInfo = sourceExpression.Type.GetMemberInfo(fieldName);

            if (memberInfo == null)
                return Expression.PropertyOrField(sourceExpression, fieldName);
            else
                return Expression.MakeMemberAccess(sourceExpression, memberInfo);
        }

        public static Expression ContainsExpression(Expression l, Expression r)
        {
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            return Expression.Call(l, method, r);
        }
    }
}
