using Microsoft.AspNetCore.Mvc;
using UrlService.Domain;

namespace UrlService.Controllers
{
  [Route("api")]
  public class UrlController : ControllerBase
  {
    private App App { get; }

    public UrlController(App app)
    {
      this.App = app;
    }

    [HttpGet("{key}")]    
    public IActionResult Get(string key)
    {
      var result = App
        .GetUrl(this.Request.Host, key)        
        .Match<IActionResult>(
          err => BadRequest(err.Message),
          v => Redirect(v.LongUrl));
      return result;
    }
  }
}
