using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

public class ProfileModel : PageModel
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";

    public void OnGet()
    {
        // Cek apakah user sudah login
        if (HttpContext.Session.GetString("UserId") == null)
        {
            Response.Redirect("/User");
            return;
        }

        // Ambil data dari session
        FullName = HttpContext.Session.GetString("FullName") ?? "";
        Email = HttpContext.Session.GetString("Email") ?? "";
    }
}
