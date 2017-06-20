using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityUnified.Contracts;
using System.Linq;

namespace MembershipCashierDL.Tests
{
    [TestClass]
    public class LowLevelAccessTests : TestBase
    {
        [TestMethod]
        public void TestFindUserProfile()
        {
            var res = SecurityDb.FindUserProfile(null);
        }

        [TestMethod]
        public void TestUpdateUserProfile()
        {
            var res = SecurityDb.FindUserProfile(null).First();
            res.UserProfile.EmailId = "way@yay.com";
            SecurityDb.UpdateUserProfile(new UserProfileContract[] { res }, res.UserProfile.UserId, true);
            res = SecurityDb.FindUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserId == res.UserProfile.UserId }).First();
        }

        [TestMethod]
        public void TestFindRole()
        {
            var res = SecurityDb.FindRole(null).First();
            var res2 = SecurityDb.FindRole(new RoleDiscriminator() { Filter = x=>x.RoleId == res.Role.RoleId }).First();
            Assert.AreEqual(res.Role.RoleName, res2.Role.RoleName);
        }
    }
}
