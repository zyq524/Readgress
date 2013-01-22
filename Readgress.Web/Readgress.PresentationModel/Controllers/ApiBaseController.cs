using Readgress.Data.Contracts;
using System;
using System.Web.Http;

namespace Readgress.PresentationModel.Controllers
{
    public abstract class ApiBaseController : ApiController
    {
        protected IReadgressUow Uow { get; set; }

        // IF IoC IS USED, WE WOULD NOT NEED THE FOLLOWING
        //
        //// base ApiController is IDisposable
        //// Dispose of the repository if it is IDisposable
        //protected override void Dispose(bool disposing)
        //{
        //    if (Uow != null && Uow is IDisposable)
        //    {
        //        ((IDisposable)Uow).Dispose();
        //        Uow = null;
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
