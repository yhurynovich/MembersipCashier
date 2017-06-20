using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityUnified.Contracts;
using System.Runtime.Serialization;
using System.IO;
using Serialize.Linq.Extensions;
using Serialize.Linq;
using SecurityUnified.Interfaces;
using Serialize.Linq.Nodes;
using System.Linq.Expressions;
using Serialize.Linq.Serializers;
using Serialize.Linq.Interfaces;
using SecurityUnified.Serialization;

namespace MembershipCashierW.Tests
{
    [TestClass]
    public class TestSerialization
    {
        //[TestMethod]
        //public void SerialiseUserProfileDiscriminator()
        //{
        //    var obj = new UserProfileDiscriminator() { Filter = x => x.UserName == "first" && x.LastName == "Manc" && x.UserId > 0 };

        //    //using(MemoryStream memStm = new MemoryStream())
        //    //{
        //    //    var serializer = new DataContractSerializer(typeof(UserProfileDiscriminator));
        //    //    serializer.WriteObject(memStm, obj);

        //    //    memStm.Seek(0, SeekOrigin.Begin);

        //    //    using(var streamReader = new StreamReader(memStm))
        //    //    {
        //    //            string result = streamReader.ReadToEnd();
        //    //    }
        //    //}

        //    //var str1 = obj.Filter.ToJson();
        //    var str2 = obj.Filter.ToString();
        //    //var str3 = obj.Filter.ToText();
        //    //var str4 = obj.Filter.ToXml();
        //    //var str5 = obj.Filter.ToExpressionNode();
        //    var deserializer = new LamdaParser<IUserProfile>();
        //    var lambda = deserializer.ParseToBooleanExpression(str2.Replace("(", "").Replace(")", ""));
        //    var z = lambda;

        //    //var factory = new Serialize.Linq.Serializers.ExpressionSerializer()
        //    //var expressionNode = factory.Create(typeof(Func<IUserProfile, bool>));
        //    //factory.

        //    //var node = new Serialize.Linq.Nodes.MemberExpressionNode()
        //    //{
        //    //    NodeType = System.Linq.Expressions.ExpressionType.MemberAccess,
        //    //    Type = new Serialize.Linq.Nodes.TypeNode()
        //    //    {
        //    //        Name = "System.String"
        //    //    },
        //    //    Member = new Serialize.Linq.Nodes.MemberInfoNode()
        //    //    {
        //    //        DeclaringType = new Serialize.Linq.Nodes.TypeNode()
        //    //        {
        //    //            Name = "SecurityUnified.Interfaces.IUserProfile",
        //    //        },
        //    //        Signature = "System.String FirstName"
        //    //    },
        //    //    Expression = new ParameterExpressionNode()
        //    //    {
        //    //        Type = new Serialize.Linq.Nodes.TypeNode()
        //    //     {
        //    //         Name = "SecurityUnified.Interfaces.IUserProfile"
        //    //     },
        //    //        NodeType = System.Linq.Expressions.ExpressionType.Parameter
        //    //    }
        //    //};


        //}

        //[TestMethod]
        //public void SerializeDeserializeGuidValue()
        //{
        //    var guidValue = Guid.NewGuid();
        //    Expression<Func<Guid>> exp = () => guidValue;

        //    SerializeAndDeserialize(exp, new XmlSerializer());  // PASSES!
        //    SerializeAndDeserialize(exp, new JsonSerializer()); // PASSES!
        //    //SerializeAndDeserialize(exp, new BinarySerializer());   // FAILS
        //}

        //public void SerializeAndDeserialize(Expression exp, ISerializer serializer)
        //{
        //    var eSerializer = new ExpressionSerializer(serializer);
        //    eSerializer.DeserializeText(eSerializer.SerializeText(exp));
        //}
    }
}
