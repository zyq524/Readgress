using System;
using System.Linq;
using Facebook;
using Microsoft.Web.WebPages.OAuth;
using Readgress.Data.Contracts;
using Readgress.Models;

namespace Readgress.PresentationModel.Utils
{
    public class FacebookLogin : IFacebookLogin
    {
        protected IReadgressUow Uow { get; set; }

        public FacebookLogin(IReadgressUow uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException("uow");
            }
            Uow = uow;
        }

        public bool Login(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException("accessToke");
            }
            var client = new FacebookClient(accessToken);
            dynamic me = client.Get("me");
            string userName = me.username;

            CreateFBReader(accessToken);

            OAuthWebSecurity.CreateOrUpdateAccount("facebook", me.id, userName);
            return OAuthWebSecurity.Login("facebook", me.id, createPersistentCookie: false);
        }

        public Reader CreateFBReader(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException("accessToke");
            }
            var client = new FacebookClient(accessToken);
            dynamic me = client.Get("me");
            string userName = me.username;
            var reader = this.Uow.Readers.GetAll().FirstOrDefault(r => r.UserName.ToLower() == userName.ToLower());
            // Check if reader already exists
            if (reader == null)
            {
                reader = new Reader()
                {
                    UserName = userName,
                    Email = me.email,
                    FirstName = me.first_name,
                    LastName = me.last_name,
                    Gender = me.gender,
                    Link = me.link,
                    CreatedOn = DateTime.Now
                };
                this.Uow.Readers.Add(reader);
                this.Uow.Commit();
            }
            return reader;
        }

        public Reader FindFBReader(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException("accessToke");
            }
            var client = new FacebookClient(accessToken);
            try
            {
                dynamic me = client.Get("me");
                string userName = me.username;
                var reader = this.Uow.Readers.GetAll().FirstOrDefault(r => r.UserName.ToLower() == userName.ToLower());
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
