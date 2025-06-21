using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

public class EditUserModel : PageModel
{
    private readonly IConfiguration _config;

    public EditUserModel(IConfiguration config)
    {
        _config = config;
    }

    [BindProperty]
    public string FullName { get; set; }

    [BindProperty]
    public string Email { get; set; }

    public IActionResult OnGet()
    {
        string? userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
            return RedirectToPage("/User");

        string connStr = _config.GetConnectionString("DefaultConnection");

        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand("SELECT FullName, Email FROM Users WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", userId);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            FullName = reader.GetString("FullName");
            Email = reader.GetString("Email");
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        string? userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
            return RedirectToPage("/User");

        if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email))
        {
            ModelState.AddModelError("", "Semua field harus diisi.");
            return Page();
        }

        string connStr = _config.GetConnectionString("DefaultConnection");

        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand("UPDATE Users SET FullName = @FullName, Email = @Email WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@FullName", FullName);
        cmd.Parameters.AddWithValue("@Email", Email);
        cmd.Parameters.AddWithValue("@Id", userId);
        cmd.ExecuteNonQuery();

        // Update session juga agar perubahan langsung terlihat
        HttpContext.Session.SetString("FullName", FullName);
        HttpContext.Session.SetString("Email", Email);

        return RedirectToPage("/Profile");
    }
}
