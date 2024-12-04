namespace Name.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            


            
            return Ok();
        }

         [HttpPost]
         [Authorize]
    public IActionResult Post(RequestModel model)
    {
        var response = new
        {
            RequestTime = DateTime.Now,
            Status = "Success"
        };
        return Ok("Success");
    }
    }
}

