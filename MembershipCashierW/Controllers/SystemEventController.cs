//using MembershipCashierW.Controllers.Authorization;
//using MembershipCashierW.Controllers.ControllerBase;
//using SecurityUnified.Serialization;
//using SecurityUnified.Serialization.Expressions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Web;
//using System.Web.Http;

//namespace MembershipCashierW.Controllers
//{
//    [RoleAuthorize(Roles = Constants.ALL_ADMINS)]
//    public class SystemEventController : WebApiControllerBase
//    {
//        [System.Web.Mvc.ValidateAntiForgeryToken]
//        public IHttpActionResult Query(string lamda)
//        {
//            return Execute(delegate
//            {
//                if (!ValidateLambdaSring(lamda))
//                    lamda = "x=>";
//                var filter = ExpressionParser.CompileBolleanFunc<ISystemEvent>(lamda);
//                var principal = SessionGlobal.CurrentPrincipal;
//                if (principal == null || !principal.RoleNames.Any(r => r == Constants.ADMIN || r == Constants.SUPER_ADMIN || r == Constants.SYSTEM))
//                {
//                    //Filter events for current session
//                    string sessionId = HttpContext.Current.Session.SessionID;
//                    Expression<Func<ISystemEvent, bool>> fa = x => x.SessionId == sessionId;
//                    filter = Expression.Lambda<Func<ISystemEvent, bool>>(Expression.AndAlso(fa.Body, filter.Body), filter.Parameters);
//                }
//                var discriminator = new SystemEventDiscriminator() { Filter = filter, Take = 3 };
//                return base.Ok( Db.FindSystemEvent(discriminator).Select(x => x.SystemEvent as SystemEventImplementor) );
//            });
//        }
//    }
//}