using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Code;
using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using MembershipCashierW.Models;
using SecurityUnified.Exceptions;
using SecurityUnified.Serialization.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;

namespace MembershipCashierW.Controllers
{
    [RoleAuthorize(Roles = Constants.USERS_WITH_PROFILE_ACCESS)]
    public class UserSnapshotController : UserProfileControllerBase
    {
        [HttpGet]
        public IEnumerable<UserSanpshotModel> Get()
        {
            try
            {
                var profiles = Db.FindUserProfile(null, AuthorizedLocationFilter, null);

                var ret = profiles.Select(p => new UserSanpshotModel()
                {
                    UserProfile2 = p.UserProfile2,
                    ProfileCredits = Db.FindProfileCredit(new ProfileCreditDiscriminator() { Filter = x => x.UserId == p.UserProfile2.UserId }).Select(x => x.ProfileCredit),
                    CreditTransactions = Db.FindCreditTransaction(new CreditTransactionDiscriminator() { Filter = x => x.UserId == p.UserProfile2.UserId, Take = 2 }).Select(x => x.CreditTransaction) //TODO: take the latest one of each product
                });

                return ret;
            }
            catch (Exception ex)
            {
                this.HandleError(ex);
                throw ex;
            }
        }

        //[EnableQuery]
        //[HttpGet]
        //public IEnumerable<UserSanpshotModel> Get(string userName) 
        //{
        //    try
        //    {
        //        var profiles = Db.FindUserProfile(new UserProfileDiscriminator() { Filter = x => x.UserName == userName }, AuthorizedLocationFilter);

        //        var ret = profiles.Select(p => new UserSanpshotModel() { 
        //             UserProfile = p.UserProfile,
        //             ProfileCredits = Db.FindProfileCredit(new ProfileCreditDiscriminator() { Filter = x => x.UserId == p.UserProfile.UserId }).Select(x=>x.ProfileCredit),
        //             CreditTransactions = Db.FindCreditTransaction(new CreditTransactionDiscriminator() { Filter = x => x.UserId == p.UserProfile.UserId, Take = 2 }).Select(x => x.CreditTransaction) //TODO: take the latest one of each product
        //        });

        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.HandleError(ex);
        //        throw ex;
        //    }
        //}

        [EnableQuery]
        [HttpGet]
        public IEnumerable<UserSanpshotModel> Query(string lamda)
        {
            try
            {
                if (!ValidateLambdaSring(lamda))
                    throw new Xxception(Errors.InvalidLambdaParameter);

                var filter = ExpressionParser.CompileBolleanFunc<IUserProfile2>(lamda);
                var discriminator = new UserProfile2Discriminator() { Filter = filter };

                var profiles = Db.FindUserProfile(discriminator, AuthorizedLocationFilter, null);

                var ret = profiles.Select(p => new UserSanpshotModel()
                {
                    UserProfile2 = p.UserProfile2,
                    ProfileCredits = Db.FindProfileCredit(new ProfileCreditDiscriminator() { Filter = x => x.UserId == p.UserProfile2.UserId }).Select(x => x.ProfileCredit),
                    CreditTransactions = Db.FindCreditTransaction(new CreditTransactionDiscriminator() { Filter = x => x.UserId == p.UserProfile2.UserId, Take = 2 }).Select(x => x.CreditTransaction) //TODO: take the latest one of each product
                });

                return ret;
            }
            catch (Exception ex)
            {
                this.HandleError(ex);
                throw ex;
            }
        }

        //// POST /api/membership/UpdateUserProfile
        //public void UpdateUserProfile([FromBody] UserProfileImplementor data)
        //{
        //    if (data == null)
        //        throw new ArgumentException("No UserProfile change data submitted");

        //    Db.UpdateUserProfile(new UserProfileContract[] { new UserProfileContract() { UserProfile = data } }, false);
        //}

        //public void UpdateProfileCredit([FromBody] ProfileCreditImplementor[] data)
        //{
        //    if (data == null || !data.Any())
        //        throw new ArgumentException("No credit change data submitted");

        //    Db.UpdateProfileCredit(data.Select(d=>
        //        new ProfileCreditContract() { ProfileCredit = d }).ToArray()
        //   , false);
        //}

        //// PUT api/membership/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/membership/5
        //public void Delete(int id)
        //{
        //}
    }
}
