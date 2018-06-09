using AN.NEOCCS.Core.Assortment.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SpeechService.Controllers
{
    [RoutePrefix("api/Speech")]
    public class HomeController : ApiController
    {

        static ConcurrentDictionary<DateTime, string> messages = new ConcurrentDictionary<DateTime, string>();

        [Route("message/{cognitiveMessage}")]
        [HttpPost]
        public async Task<IHttpActionResult> AddMessage(string cognitiveMessage)
        {
            HttpResponseMessage message;

            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                message = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                message = Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            }

            return ResponseMessage(message);
        }

        [Route("message")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMessage()
        {
           HttpResponseMessage message;

            try
            {
                return Ok("The glass is half full, not half empty");
            }
            catch (Exception ex)
            {
                message = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch
            {
                message = Request.CreateResponse(HttpStatusCode.BadRequest, "error");
            }

            return ResponseMessage(message);
        }
    }
}
