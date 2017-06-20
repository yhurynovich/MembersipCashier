using Serialize.Linq.Extensions;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SecurityUnified.Serialization
{
    //TODO: Handle join operators
    public class LamdaParser<T> //where T : object
    {
        private static Regex paramNameRegex = new Regex(@"\s*([\w]+)\s*=>", RegexOptions.Multiline | RegexOptions.Compiled);
        public static string[] joinOperators = new string[] { "&&", "||" };
        private static Regex ruleRegex = new Regex(@"\s*(([^&|]+)\s*(&&|\|\|)?)\s*", RegexOptions.Multiline | RegexOptions.Compiled);
        private static Regex rulePartRegex = new Regex(@"\s*([^=><!\s]+)\s*([=><!~]+)\s*(.+)", RegexOptions.Multiline | RegexOptions.Compiled);

        public Expression<Func<T, bool>> ParseToBooleanExpression(string lambdaExpresiionString)
        {
            if (lambdaExpresiionString.Contains("(") || lambdaExpresiionString.Contains(")"))
                throw new Xxception("LamdaParser do not support () brakets");

            lambdaExpresiionString = lambdaExpresiionString.Replace("AndAlso", "&&").Replace("OrElse","||");

            var paramNameMatch = paramNameRegex.Match(lambdaExpresiionString);
            if (paramNameMatch.Success)
            {
                string paramName = paramNameMatch.Groups[1].Value;
                string selectorStr = paramNameRegex.Replace(lambdaExpresiionString, String.Empty);

                Type tType = typeof(T);
                var param = Expression.Parameter(tType, paramName);

                BinaryExpression rootExpression = null;
                BinaryExpression leftExpression = null; //contains previously parsed rule(s) in first parameter
                var ruleMatch = ruleRegex.Match(selectorStr);
                while (ruleMatch.Success)
                {
                    BinaryExpression rightExpression = GetRuleExpression(param, ruleMatch.Groups[2].Value);
                    if (ruleMatch.Groups.Count > 3 && !string.IsNullOrEmpty(ruleMatch.Groups[3].Value))
                    {
                        rightExpression = GetLogicalExpression(ruleMatch.Groups[3].Value, rightExpression, null);
                    }

                    if (leftExpression == null)
                    {
                        rootExpression = leftExpression = rightExpression;
                    }
                    else
                    {
                        //TODO: improve ths block
                        if (object.ReferenceEquals(rootExpression, leftExpression))
                            rootExpression = leftExpression = leftExpression.Update(leftExpression.Left, leftExpression.Conversion, rightExpression);
                        else
                            leftExpression = leftExpression.Update(leftExpression.Left, leftExpression.Conversion, rightExpression);
                        leftExpression = rightExpression;
                    }

                    ruleMatch = ruleMatch.NextMatch();
                }

                if (rootExpression == null) // No rules supplies
                    rootExpression = Expression.OrElse(Expression.Constant(true), Expression.Constant(true)); //Return true

                var lambda = Expression.Lambda<Func<T, bool>>(rootExpression, param);

                return lambda;
            }
            throw new Xxception(string.Format("Unable to parse expression {0}" ,lambdaExpresiionString));
        }

        /// <summary>
        /// 
        /// <param name="param">ParameterExpression that points to "x=>" parameter</param>
        /// <param name="part">part of the lambda rule like "x.Id = 1"</param>
        /// <returns></returns>
        private BinaryExpression GetRuleExpression(ParameterExpression param, string part)
        {
            var partMatch = rulePartRegex.Match(part);
            if (partMatch.Success)
            {
                Type tType = typeof(T);
                string fieldName = partMatch.Groups[1].Value.Replace("(", string.Empty).Replace(param.Name + ".", string.Empty).Trim();
                //var fieldNameParts = fieldName.Split('.');
                //fieldName = fieldNameParts[fieldNameParts.Length - 1];

                string compareOperator = partMatch.Groups[2].Value.Trim();
                string compareTo = partMatch.Groups[3].Value.Replace(")", string.Empty).Trim();

                if (compareTo.StartsWith("\"") && compareTo.EndsWith("\""))
                {
                    compareTo = compareTo.Substring(1, compareTo.Length - 2);
                }

                System.Reflection.MemberInfo memberInfo = null;
                var fieldNameParts = fieldName.Split('.');
                string memberStrName = fieldNameParts[fieldNameParts.Length - 1];
                foreach (var interfType in tType.GetInterfaces())
                {
                    var memberInfos = interfType.GetMember(memberStrName, System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    if (memberInfos.Length > 0)
                    {
                        memberInfo = memberInfos[0]; //CONSIDER: multiple fields can be found
                    }
                }
                if (memberInfo == null)
                {
                    var memberInfos = tType.GetMember(memberStrName, System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    if (memberInfos.Length > 0)
                    {
                        memberInfo = memberInfos[0]; //CONSIDER: multiple fields can be found
                    }
                }

                if (memberInfo == null)
                    throw new Xxception(string.Format("Type '{0}' do not expose member named '{1}'", tType.FullName, memberStrName));
                Type compareToType = memberInfo.GetReturnType();

                var lhs = Expression.MakeMemberAccess(param, memberInfo);
                ConstantExpression rhs;
                if (string.IsNullOrEmpty(compareTo) && compareToType.IsValueType)
                {
                    var val = Activator.CreateInstance(compareToType);
                    rhs = Expression.Constant(val);
                }
                else
                {
                    //if (compareToType.GenericTypeArguments != null && compareToType.GenericTypeArguments.Length == 1) //Nullable type
                    //{
                    //    var underlyingType = compareToType.GenericTypeArguments[0];
                    //    var val = Convert.ChangeType(compareTo, underlyingType);
                    //    rhs = Expression.Constant(val);
                    //}
                    //else
                    //{
                    //    var val = Convert.ChangeType(compareTo, compareToType);
                    //    rhs = Expression.Constant(val);
                    //}
                    var val =ChangeType(compareTo, compareToType);
                    rhs = Expression.Constant(val);
                }
                return GetCompareExpression(compareOperator, lhs, rhs);
            }
            throw new Xxception(string.Format("Unable to parse lambda expression {0}", part));
        }

        private BinaryExpression GetLogicalExpression(string opr, BinaryExpression left, Expression right)
        {
            if (right == null)
                right = Expression.Constant(true);

            switch (opr)
            {
                case "AndAlso":
                case "&&":
                    return Expression.AndAlso(left, right);
                case "OrElse":
                case "||":
                    return Expression.OrElse(left, right);
                case "And":
                case "&":
                    return Expression.And(left, right);
                case "Or":
                case "|":
                    return Expression.Or(left, right);
                default:
                    throw new Xxception("Unknown logical operator '" + opr + "'");
            }
        }

        private BinaryExpression GetCompareExpression(string opr, MemberExpression left, Expression right)
        {
            if (!left.Type.Equals(right.Type))
            {
                right = Expression.Convert(right, left.Type);
            }

            switch (opr)
            {
                case "==":
                case "Equal":
                    return Expression.Equal(left, right);
                case ">":
                case "GreaterThan":
                    return Expression.GreaterThan(left, right);
                case "<":
                case "LessThan":
                    return Expression.LessThan(left, right);
                case ">=":
                case "GreaterThanOrEqual":
                    return Expression.GreaterThanOrEqual(left, right);
                case "<=":
                case "LessThanOrEqual":
                    return Expression.LessThanOrEqual(left, right);
                case "!=":
                case "NotEqual":
                    return Expression.NotEqual(left, right);
                case "~":
                case "like":
                    return SecurityUnified.Serialization.ExpressionExtentions.Contains<string,MemberExpression,ConstantExpression>(left, (ConstantExpression) right);
                default:
                    throw new Xxception("Unknown comparation operator '" + opr + "'");
            }
        }

        /// <summary>
        /// Returns underlying type value of object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (value as string == "null")
                value = null;

            if (value == null)
            {
                if (conversionType == typeof(System.String))
                    return string.Empty;
                else
                    return Activator.CreateInstance(conversionType);
            }

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type underlyingType = conversionType;
                if (value == null)
                    return null;
                var nullableConverter = new NullableConverter(conversionType);
                underlyingType = nullableConverter.UnderlyingType;
                return Convert.ChangeType(value, underlyingType);
            }
            else
                return Convert.ChangeType(value, conversionType);
        }

        //public static object ChangeType(object value, Type conversion)
        //{
        //    var t = conversion;

        //    if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        //    {
        //        if (value == null)
        //        {
        //            return null;
        //        }

        //        t = Nullable.GetUnderlyingType(t);
        //        var ret = Activator.CreateInstance(conversion);
        //        conversion.GetField("Value").SetValue(ret, Convert.ChangeType(value, t));
        //        return ret;
        //    }
        //    else
        //        return Convert.ChangeType(value, t);
        //}


        //private Type GetNullableType(Type type)
        //{
        //    type = Nullable.GetUnderlyingType(type);
        //    if (type.IsValueType)
        //        return typeof(Nullable<>).MakeGenericType(type);
        //    else
        //        return type;
        //}
    }
}
