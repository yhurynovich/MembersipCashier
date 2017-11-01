using MembershipCashierDL.Access;
using MembershipCashierW.Code;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace MembershipCashierW.Controllers.ControllerBase
{
    public abstract class WebApiControllerBase : ApiController
    {
        private static LowLevelAccess db;
        private static SecurityDL.Access.LowLevelAccess securitydb;

        public LowLevelAccess Db
        {
            get
            {
                if (db == null)
                    db = new LowLevelAccess();
                return db;
            }
        }

        private T HandleError<T>(Exception ex)
        {
            if (typeof(T) == typeof(IHttpActionResult) || typeof(T).IsInstanceOfType(typeof(IHttpActionResult)))
            {
                Utils.WriteToEventLog(ex);
                IHttpActionResult res;
                if (typeof(System.Web.Mvc.HttpAntiForgeryException).IsInstanceOfType(ex))
                    res = base.Unauthorized();
                else
                    res = base.InternalServerError(ex);
                return (T)res;
            }
            else if (typeof(T) == typeof(HttpResponseMessage) || typeof(T).IsInstanceOfType(typeof(HttpResponseMessage)))
            {
                Utils.WriteToEventLog(ex);
                HttpResponseMessage res;
                if (typeof(System.Web.Mvc.HttpAntiForgeryException).IsInstanceOfType(ex))
                    res = Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, ex);
                else
                    res = Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex);
                return (T)Convert.ChangeType(res, typeof(HttpResponseMessage));
            }
            else
            {
                Utils.HandleError(this, ex);
                throw ex; // Never executed but needed to satisfy compiler
            }
        }

        protected T Execute<T>(Func<T> action)
        {
            try
            {
                if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != "~/api/authentication/login")
                    Authorization.Antiforgery.Services.ServiceAntiforgery.Assert();
                return action.Invoke();
            }
            catch (Exception cex)
            {
                return HandleError<T>(cex);
            }
        }

        protected void Execute(Action action)
        {
            try
            {
                if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != "~/api/authentication/login")
                    Authorization.Antiforgery.Services.ServiceAntiforgery.Assert();
                action.Invoke();
            }
            catch (Exception cex)
            {
                HandleError<object>(cex);
            }
        }

        public override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Web.Http.Controllers.HttpControllerContext controllerContext, System.Threading.CancellationToken cancellationToken)
        {
            return base.ExecuteAsync(controllerContext, cancellationToken).ContinueWith(t =>
            {
                // the controller action has finished executing, 
                // lets inject caching policy headers

                System.Net.Http.HttpResponseMessage ret = null;

                try
                {
                    ret = t.Result;

                    if (ret != null && ret.Headers != null)
                    {
                        ret.Headers.CacheControl = new CacheControlHeaderValue
                        {
                            Public = false,
                            MaxAge = TimeSpan.FromMilliseconds(100),
                            MustRevalidate = true,
                            NoCache = true
                        };
                    }
                }
                catch (AggregateException aex) //Ignore task TaskCanceledException
                {
                    if (!(aex.InnerException != null && typeof(System.Threading.Tasks.TaskCanceledException).IsInstanceOfType(aex.InnerException)))
                    {
                        var cnt = aex.InnerExceptions.Count;
                        if (cnt > 1)
                        {
                            for (int i = 0; i < cnt - 1; i++)
                            {
                                Utils.WriteToEventLog(aex.InnerExceptions[i]);
                            }
                        }
                        return GenericErrorRedirect(aex.InnerExceptions[cnt - 1]);
                    }
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    //Ignore task TaskCanceledException
                }
                catch (Exception ex)
                {
                    return GenericErrorRedirect(ex);
                }

                return ret;
            });
        }

        private System.Net.Http.HttpResponseMessage GenericErrorRedirect(Exception ex)
        {
#if DEBUG
            this.HandleError(ex);
            return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "");
#else
            if (Request.Headers.Contains("detailedErrors"))
            {
                this.HandleError(ex);
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "");
            }
            else
            {
                Utils.WriteToEventLog(ex);

                var response = Request.CreateResponse(System.Net.HttpStatusCode.RedirectKeepVerb);
                response.Headers.Location = new Uri(Request.RequestUri, "/Shared/Error");

                return response;
            }
#endif
        }

        public SecurityDL.Access.LowLevelAccess SecurityDb
        {
            get
            {
                if (securitydb == null)
                    securitydb = new SecurityDL.Access.LowLevelAccess();
                return securitydb;
            }
        }

        protected bool ValidateLambdaSring(string lambda)
        {
            return !string.IsNullOrEmpty(lambda) && lambda.IndexOf("=>") > 0;
        }

        protected override void Dispose(bool disposing)
        {
            db = null;
            securitydb = null;

            base.Dispose(disposing);
        }
    }
}