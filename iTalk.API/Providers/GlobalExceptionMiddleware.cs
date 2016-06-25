using iTalk.API.Properties;
using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace iTalk.API.Providers {
    /// <summary>
    /// An Owin Middleware Handle Exceptions
    /// </summary>
    public class GlobalExceptionMiddleware : OwinMiddleware {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="next">next middleware</param>
        public GlobalExceptionMiddleware(OwinMiddleware next) : base(next) { }

        /// <summary>
        /// Catch Middleware Exceptions, Return Custom Response
        /// </summary>
        /// <param name="context">OwinContext</param>
        /// <returns>Task</returns>
        public override async Task Invoke(IOwinContext context) {
            try {
                await Next.Invoke(context);
            }
            catch (Exception) {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(Resources.UnkwonError);
            }
        }
    }
}