using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDL.Access;
using SecurityUnified.Contracts;

namespace SecurityDL.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var access = new LowLevelAccess();
            var res = access.FindUserProfile(new UserProfileDiscriminator() {  Filter = x=>x.UserId == 2 });
        }
    }
}
