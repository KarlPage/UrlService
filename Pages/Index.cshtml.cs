using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using UrlService.Domain;

namespace UrlService.Pages
{
  public class IndexModel : PageModel
  {
    private App App { get; }

    [Required(AllowEmptyStrings=false, ErrorMessage="Please enter a valid url")]
    [BindProperty]
    public string InputUrl { get; set; }

    public Result<UrlResult> UrlEntryResult { get; set; }

    public IndexModel(App app)
    {
      this.App = app;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
      if (!ModelState.IsValid)
        return Page();
      
      this.UrlEntryResult = App.AddUrlKey(this.Request.Host, InputUrl);
      return Page();
    }
  }
}
