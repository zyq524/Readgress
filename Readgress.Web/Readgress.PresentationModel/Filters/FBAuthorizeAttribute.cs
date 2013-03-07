using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Readgress.Models;
using Readgress.PresentationModel.Utils;

namespace Readgress.PresentationModel.Filters
{
    /// <summary>
    /// Customized AuthorizeAttribute using FacebookAccessToken.
    /// If the request path is /api/reader, create a new reader if that reader is not found.
    /// For the other requests, deny the request if the reader is not found.
    /// </summary>
    public class FBAuthorizeAttribute : AuthorizeAttribute
    {
        protected IFacebookLogin FBLogin { get; set; }

        public FBAuthorizeAttribute()
        {
            FBLogin = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IFacebookLogin)) as IFacebookLogin;
        }

        bool requireSsl = false;

        public bool RequireSsl
        {
            get { return requireSsl; }
            set { requireSsl = value; }
        }
            bool requireAuthentication = true;

        public bool RequireAuthentication
        {
            get { return requireAuthentication; }
            set { requireAuthentication = value; }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //No FacebookAccessToken in the header, just do the standard way.
            if (!HttpContext.Current.Request.Headers.AllKeys.Contains("FacebookAccessToken"))
            {
                base.OnAuthorization(actionContext);
            }

            else if (AuthenticateByFacebookAccessToken(actionContext) || !RequireAuthentication)
            {
                return;
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }

        private bool AuthenticateByFacebookAccessToken(HttpActionContext actionContext)
        {
            if (RequireSsl && !HttpContext.Current.Request.IsSecureConnection && !HttpContext.Current.Request.IsLocal)
            {
                //TODO: Return false to require SSL in production - disabled for testing before cert is purchased
                return false;
            }

            if (!HttpContext.Current.Request.Headers.AllKeys.Contains("FacebookAccessToken"))
            {
                return false;
            }

            string accessToken = HttpContext.Current.Request.Headers["FacebookAccessToken"];

            IPrincipal principal;
            if (TryGetPrincipal(accessToken, actionContext, out principal))
            {
                HttpContext.Current.User = principal;
                return true;
            }
            return false;
        }

        private bool TryGetPrincipal(string accessToken, HttpActionContext actionContext, out IPrincipal Principal)
        {
            Reader reader = null;
            try
            {
                //is valid username?
                reader = actionContext.Request.RequestUri.AbsolutePath.StartsWith("/api/reader") ? this.FBLogin.CreateFBReader(accessToken) : this.FBLogin.FindFBReader(accessToken);
            }
            catch (Exception ex)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                response.Content = new StringContent(ex.Message);
                throw new HttpResponseException(response);
            }

            if (reader != null)
            {
                //check if in allowed role
                //bool isAllowedRole = false;
                string[] userRoles = System.Web.Security.Roles.GetRolesForUser(reader.UserName);
                //string[] allowedRoles = Roles.Split(',');  //Roles is the inherited AuthorizeAttribute.Roles member
                //foreach(string userRole in userRoles)
                //{
                //    foreach (string allowedRole in allowedRoles)
                //    {
                //        if (userRole == allowedRole)
                //        {
                //            isAllowedRole = true;
                //        }
                //    }
                //}

                //if (!isAllowedRole)
                //{
                //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                //}

                //Principal = new GenericPrincipal(new GenericIdentity(user.UserName), userRoles);                
                Principal = new GenericPrincipal(new GenericIdentity(reader.UserName), userRoles);
                Thread.CurrentPrincipal = Principal;

                return true;
            }
            else
            {
                Principal = null;
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
        }
    }
}
