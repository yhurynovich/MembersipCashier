//using Serialize.Linq.Interfaces;
//using Serialize.Linq.Serializers;
//using System;
//using System.Linq;
//using System.IO;
//using System.Runtime.Serialization;
//using System.Text.RegularExpressions;
//using System.Linq.Expressions;

//namespace SecurityUnified.Serialization
//{
//    public class LambdaSerializer : SerializerBase, ITextSerializer, ISerializer
//    {
//        private static Regex paramNameRegex = new Regex(@"\s*([\w]+)\s*=>", RegexOptions.Multiline | RegexOptions.Compiled);
//        public static string[] joinOperators = new string[] { "&&", "||" };
//        //public static string[] compareOperators = new string[] { "==", ">", "<", ">=", "<=", "!=", "===", "!===" };
//        private static Regex partRegex = new Regex(@"\s*([^=><!\s]+)\s*([=><!]+)\s*(.+)", RegexOptions.Multiline | RegexOptions.Compiled);

//        public T Deserialize<T>(string text) where T : Serialize.Linq.Nodes.Node
//        {
//            var paramNameMatch = paramNameRegex.Match(text);
//            if (paramNameMatch.Success)
//            {
//                string paramName = paramNameMatch.Groups[1].Value;
//                string selectorStr = paramNameRegex.Replace(text, String.Empty);
//                var parts = selectorStr.Split(joinOperators, StringSplitOptions.RemoveEmptyEntries);
//                foreach (var part in parts)
//                {
//                    var partMatch = partRegex.Match(part);
//                    if (partMatch.Success)
//                    {
//                        string fieldName = partMatch.Groups[1].Value.Replace("(",string.Empty).Replace(paramName+".", string.Empty);
//                        string compareOperator = partMatch.Groups[2].Value;
//                        string compareTo = partMatch.Groups[3].Value.Replace(")", string.Empty);
//                    }

//                    var node = Expression<Func<T, bool>>.;
//                    node.ToBooleanExpression();
//                }
//            }
//            return default(T);
//        }

        
//        public T Deserialize<T>(Stream stream) where T : Serialize.Linq.Nodes.Node
//        {
//            string allText;
//            using (var reader = new StreamReader(stream))
//            {
//                allText = reader.ReadToEnd();
//            }
//            return Deserialize<T>(allText);
//        }

//        public string Serialize<T>(T obj) where T : Serialize.Linq.Nodes.Node
//        {
//            throw new NotImplementedException();
//        }


//        public void Serialize<T>(Stream stream, T obj) where T : Serialize.Linq.Nodes.Node
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
