using System;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MembershipCashierW.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        internal class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                //Database.SetInitializer<DB.UsersDBDBEntities>(null);

                try
                {
                    //UsersLqDBPool.PoolItem pi = null;
                    //try
                    //{
                    //    pi = UsersLqDBPool.Instance.GetDB();
                    //    var context = pi.Item;
                    //    if (!context.DatabaseExists())
                    //    {
                    //        // Create the SimpleMembership database without Entity Framework migration schema
                    //        context.CreateDatabase();
                    //    }
                    //}
                    //finally
                    //{
                    //    UsersLqDBPool.Instance.Return(pi);
                    //}

                    //This opens database and creates tables
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}