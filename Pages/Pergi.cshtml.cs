using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Pergi.com.Pages
{
    public class PergiModel : PageModel
    {
        private readonly ILogger<PergiModel> _logger;

        public PergiModel(ILogger<PergiModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
