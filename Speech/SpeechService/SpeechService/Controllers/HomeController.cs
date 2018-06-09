using AN.NEOCCS.Core.Assortment.Models;
using System;
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
 
        //[Route("insert")]
        //[HttpPost]
        //public async Task Insert()
        //{

        //}

        [Route("message")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSpeechMessage()
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
