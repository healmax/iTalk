using iTalk.API.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace iTalk.API {
    class GlobalExceptionFilterAttribute : ExceptionFilterAttribute {
        public override void OnException(HttpActionExecutedContext actionExecutedContext) {
            ExecuteResult result = new ExecuteResult(false, (int)HttpStatusCode.BadRequest, actionExecutedContext.Exception.Message);
            actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, result);
        }
    }
}