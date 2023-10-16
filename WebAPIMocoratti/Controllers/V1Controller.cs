using Microsoft.AspNetCore.Mvc;

namespace WebAPIMocoratti.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/teste")]

    [ApiController]
    public class Versao1Controller : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>V 1.O</h2></body></html>", "text/html");
        }
       
    }
}
