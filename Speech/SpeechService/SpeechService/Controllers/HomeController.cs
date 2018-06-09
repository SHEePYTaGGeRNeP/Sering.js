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

        static ConcurrentDictionary<DateTime, string> speechMessages = new ConcurrentDictionary<DateTime, string>();
        static ConcurrentDictionary<DateTime, string> hueMessages = new ConcurrentDictionary<DateTime, string>();

        [Route("message/drink/{beverageType}/volume/{volume}")]
        [HttpPost]
        public async Task<IHttpActionResult> AddMessage(string beverageType, string volume)
        {
            HttpResponseMessage message;

            try
            {
                speechMessages.TryAdd(DateTime.UtcNow, beverageType + "_" + volume);
                hueMessages.TryAdd(DateTime.UtcNow, beverageType + "_" + volume);
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

        [Route("speechmessage")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSpeechMessage()
        {
           HttpResponseMessage message;
            string returnMessage = "";

            try
            {
                KeyValuePair<DateTime, string> speechMessage;

                if (speechMessages.Count > 0)
                {
                    speechMessage = speechMessages.OrderByDescending(x => x.Key).FirstOrDefault();
                    var test = "";
                    speechMessages.TryRemove(speechMessage.Key, out test);
                } else
                {
                    return Ok("empty");
                }
                
                return Ok(speechMessage.Value);
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

        [Route("huemessage")]
        [HttpGet]
        public async Task<IHttpActionResult> GetHueMessage()
        {
            HttpResponseMessage message;

            try
            {
                KeyValuePair<DateTime, string> hueMessage;

                if (hueMessages.Count > 0)
                {
                    hueMessage = hueMessages.OrderByDescending(x => x.Key).FirstOrDefault();
                    var test = "";
                    hueMessages.TryRemove(hueMessage.Key, out test);
                }
                else
                {
                    return Ok("empty");
                }

                return Ok(hueMessage.Value);
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
