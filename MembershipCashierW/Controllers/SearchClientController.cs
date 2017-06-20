using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using System;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    /// <summary>
    /// Search Client Controller
    /// </summary>
    [RoleAuthorize(Roles = Constants.USERS_WITH_PROFILE_ACCESS)]
    public class SearchClientController : BlobSearchUserProfileControllerBase
    {
        /// <summary>
        /// Searches userprofiles by applying blob against email, first name, last name and phone fields. Only profiles for current location and in role "client" will be returned
        /// </summary>
        /// <param name="blob">text to search by</param>
        /// <param name="skip">skip result records</param>
        /// <param name="take">number of records to take</param>
        /// <returns></returns>
        public IHttpActionResult Get(string blob, int? skip, int? take)
        {
            return Execute<IHttpActionResult>(delegate
            {
                UserProfile2Discriminator mainDiscriminator = new UserProfile2Discriminator();

                if (!string.IsNullOrWhiteSpace(blob))
                    mainDiscriminator.Filter = base.GetBlobFilter(blob);

                if (skip.HasValue)
                    mainDiscriminator.Skip = skip.Value;
                if (take.HasValue)
                    mainDiscriminator.Take = take.Value;

                return base.Ok(Db.FindUserProfile(
                    mainDiscriminator, 
                    AuthorizedLocationFilter,
                    new SecurityUnified.Contracts.RoleDiscriminator() { Filter = x=>x.RoleName == "client" }
                ).Select(x => x.UserProfile2 as UserProfile2Implementor));
            });
        }
    }
}