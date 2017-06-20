using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using SecurityUnified.Contracts;
using SecurityUnified.Exceptions;
using SecurityUnified.Interfaces;
using SecurityUnified.Serialization;
using SecurityUnified.Serialization.Expressions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;

namespace MembershipCashierW.Controllers
{
    public enum InserUsersConflictStrategy
    {
        Default = 0,
        ForceCreate = 1
    }

    [DataContract]
    public struct InserUsersRequest
    {
        [DataMember]
        public UserProfileImplementor[] Profiles { get; set; }
        [DataMember]
        public InserUsersConflictStrategy ConflictResolutionStartegy { get; set; }
    }

    [RoleAuthorize(Roles = Constants.USERS_WITH_PROFILE_ACCESS)]
    public class UserProfileController : UserProfileControllerBase
    {
        public IHttpActionResult Get()
        {
            return Execute<IHttpActionResult>(delegate
            {
                return base.Ok(Db.FindUserProfile(null, AuthorizedLocationFilter, null).Select(x => x.UserProfile2 as UserProfileImplementor));
            });
        }

        public IHttpActionResult Get(string userName)
        {
            return Execute<IHttpActionResult>(delegate
            {
                return base.Ok(Db.FindUserProfile(new UserProfile2Discriminator() { Filter = x => x.UserName == userName }, AuthorizedLocationFilter, null).Select(x => x.UserProfile2 as UserProfile2Implementor));
            });
        }

        [HttpGet]
        public IHttpActionResult Query(string lamda)
        {
            return Execute<IHttpActionResult>(delegate
            {
                if (!ValidateLambdaSring(lamda))
                    throw new Xxception(Errors.InvalidLambdaParameter);

                var filter = ExpressionParser.CompileBolleanFunc<IUserProfile2>(lamda);
                var discriminator = new UserProfile2Discriminator() { Filter = filter };
                return base.Ok(Db.FindUserProfile(discriminator, AuthorizedLocationFilter, null).Select(x => x.UserProfile2 as UserProfile2Implementor));
            });
        }

        /// <summary>
        /// Updates userprofile by userId
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] UserProfileImplementor[] data)
        {
            return Execute(delegate
            {
                if (data == null || !data.Any())
                    throw new ArgumentException("No UserProfile change data submitted");

                if (data.Any(d => d == null || d.UserId <= 0 || string.IsNullOrWhiteSpace(d.UserName)))
                    throw new ArgumentException("User data is incomplete");

                if (SessionGlobal.CurrentUser == null || SessionGlobal.CurrentUser.Identity == null || !SessionGlobal.CurrentUser.Identity.IsAuthenticated)
                    throw new Exception("Current user is not authorized to make changes to profile");

                SecurityDb.UpdateUserProfile(data.Select(d =>
                    new UserProfileContract() { UserProfile = d }).ToArray()
               , SessionGlobal.CurrentUser.Identity.UserId, false);

                return Request.CreateResponse(HttpStatusCode.Accepted);
            });
        }

        /// <summary>
        /// This method ignores UserId and UserName
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="expParam"></param>
        /// <returns></returns>
        private Expression<Func<IUserProfile, bool>> ToPartialLambda( IUserProfile filter, ParameterExpression expParam)
        {
            if (filter == null)
                return x => false;

            Expression res = null;
            //Expression<Func<IUserProfile, bool>> ret = x => true;
            //var expParam = ret.Parameters.First();

            if (!string.IsNullOrEmpty(filter.EmailId))
                if(res==null)
                    res = Expression.Equal(expParam.GetFieldAccessExpression("EmailId"), Expression.Constant(filter.EmailId));
                else
                    res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("EmailId"), Expression.Constant(filter.EmailId)), res);
            if (!string.IsNullOrEmpty(filter.FirstName))
                if (res == null)
                    res = Expression.Equal(expParam.GetFieldAccessExpression("FirstName"), Expression.Constant(filter.FirstName));
                else
                    res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("FirstName"), Expression.Constant(filter.FirstName)), res);
            if (!string.IsNullOrEmpty(filter.LastName))
                if (res == null)
                    res = Expression.Equal(expParam.GetFieldAccessExpression("LastName"), Expression.Constant(filter.LastName));
                else
                    res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("LastName"), Expression.Constant(filter.LastName)), res);
            if (!string.IsNullOrEmpty(filter.Phone))
                if (res == null)
                    res = Expression.Equal(expParam.GetFieldAccessExpression("Phone"), Expression.Constant(filter.Phone));
                else
                    res = Expression.AndAlso(Expression.Equal(expParam.GetFieldAccessExpression("Phone"), Expression.Constant(filter.Phone)), res);

            return Expression.Lambda<Func<IUserProfile, bool>>(res, expParam);
        }

        /// <summary>
        /// Inserts or upfdatescUserProfile
        /// </summary>
        /// <param name="data"></param>
        /// <returns>[IUserProfile] affected profiles with userIds</returns>
        [HttpPut]
        public HttpResponseMessage Put([FromBody] InserUsersRequest data)
        {
            return Execute(delegate
            {
                if (data.Profiles == null || !data.Profiles.Any())
                    throw new ArgumentException("No UserProfile change data submitted");

                if (data.Profiles.Any(d => d == null || string.IsNullOrWhiteSpace(d.UserName)))
                    throw new ArgumentException("User data is incomplete");

                if (SessionGlobal.CurrentUser == null || SessionGlobal.CurrentUser.Identity == null || !SessionGlobal.CurrentUser.Identity.IsAuthenticated)
                    throw new Exception("Current user is not authorized to make changes to profile");

                //First, lets identify if similar profile exists
                Expression<Func<IUserProfile, bool>> ret = x => false;
                var expParam = ret.Parameters.First();
                Expression res = ret.Body;
                foreach (var ru in data.Profiles)
                {
                    res = Expression.OrElse(Expression.Invoke(ToPartialLambda(ru, expParam), expParam), Expression.Invoke(ret, expParam));
                }

                var similar = SecurityDb.FindUserProfile(new UserProfileDiscriminator() { Filter = Expression.Lambda<Func<IUserProfile, bool>>(res, expParam) });    

                //First resolve conflicts
                if (similar.Any())
                {
                    switch (data.ConflictResolutionStartegy)
                    {
                        case InserUsersConflictStrategy.Default:
                            return Request.CreateResponse(HttpStatusCode.Conflict, similar.Select(x => x.UserProfile));
                        //case InserUsersConflictStrategy.ForceCreate:
                        //    //Now we can insert new profiles
                        //    SecurityDb.InsertUserProfile(similar.Select(d =>
                        //        new ExtendedUserProfileContract() {
                        //            UserProfile = d
                        //            , 
                        //        }).ToArray());
                        //    break;
                    }
                }

                //Second insert the rest


                return Request.CreateResponse(HttpStatusCode.Accepted);
            });
        }
    }
}