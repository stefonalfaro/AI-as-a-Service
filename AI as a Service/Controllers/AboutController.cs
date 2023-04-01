using Microsoft.AspNetCore.Mvc;

namespace AI_as_a_Service.Controllers
{
    public class AboutResponse
    {
        public string APIVersion { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class AboutController : ControllerBase
    {
        [HttpGet]
        public ActionResult<AboutResponse> GetAPIVersion()
        {
            var response = new AboutResponse
            {
                APIVersion = "1.0.2"
            };

            return Ok(response);
        }
    }
}
