using Microsoft.AspNetCore.Mvc;
using TwitterBook.Filters;

namespace TwitterBook.Controllers.V1;

[ApiKeyAuth]
public class SecretController : ControllerBase
{
    [HttpGet("secret")]
    public IActionResult GetSecret()
    {
        return Ok("i have no secrets");
    }
}