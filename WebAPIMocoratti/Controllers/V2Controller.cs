using Microsoft.AspNetCore.Mvc;

namespace WebAPIMocoratti.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/teste")]
    [ApiController]
    public class VersaoController : Controller
    {
        [HttpGet("Versao2")]
        public IActionResult Get()
        {
            return Content("<html><body><h2>V 2.O</h2></body></html>", "text/html");
        }
    }
}
