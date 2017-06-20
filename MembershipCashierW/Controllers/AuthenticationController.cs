using MembershipCashierW.Code;
using MembershipCashierW.Controllers.Authorization;
using MembershipCashierW.Controllers.ControllerBase;
using MembershipCashierW.Models;
using System;
using System.Net.Http.Headers;
using System.Web.Http;

/// <summary>
/// Authentication Controller
/// </summary>
public class AuthenticationController : WebApiControllerBase
{
    /// <summary>
    /// Lazy loading getter for core authentication functions
    /// </summary>
    public AuthenticationFactory AuthenticationFactory
    {
        get
        {
            if (_AuthenticationFactory == null) _AuthenticationFactory = new AuthenticationFactory();

            return _AuthenticationFactory;
        }
    }
    private AuthenticationFactory _AuthenticationFactory;

    /// <summary>
    /// Application Log-In method for all non-impersonated users
    /// </summary>
    /// <param name="model">WebApiLoginModel</param>
    /// <returns>Ok</returns>
    [HttpPost]
    [AllowAnonymous]
    public IHttpActionResult Login([FromBody] LoginModel model)
    {
        try
        {
            //Inject ldap information collected by browser
            if (System.Web.HttpContext.Current != null
                && System.Web.HttpContext.Current.Request != null
                && System.Web.HttpContext.Current.Request.LogonUserIdentity != null
                && typeof(System.Security.Principal.WindowsIdentity).IsInstanceOfType(System.Web.HttpContext.Current.Request.LogonUserIdentity)
                )
                model.LdapAccount = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name;

            var result = AuthenticationFactory.Login(model);

            if (result)
                return Ok(new { success = true, suggestedUrl = new MembershipCashierW.Code.UrlRouting.UrlRouter().GetEntryPage() });

            return Unauthorized();
        }
        catch (AuthenticationFactory.UnauthorizedException ex)
        {
            if (!ex.GenerateCaptcha)
                return Unauthorized();

            return Unauthorized(new AuthenticationHeaderValue("captcha_img", _AuthenticationFactory.GenerateCaptcha()));
        }
        catch (Exception ex)
        {
            this.HandleError(ex);
            return Unauthorized();
        }
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns>Ok</returns>
    [HttpGet]
    [AllowAnonymous]
    public IHttpActionResult Logout()
    {
        AuthenticationFactory.Logout();
        return Ok(new { success = true });
    }
}

