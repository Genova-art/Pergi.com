using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

public class DeleteUserModel : PageModel
{
    private readonly IConfiguration _config;

    public DeleteUserModel(IConfiguration config)
    {
        _config = config;
    }

    public IActionResult OnPost()
    {
        string? userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
            return RedirectToPage("/User");

        string connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new MySqlConnection(connStr);
        conn.Open();

        // Hapus user
        var cmd = new MySqlCommand("DELETE FROM Users WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", userId);
        cmd.ExecuteNonQuery();

        // Hapus session dan redirect ke halaman login
        HttpContext.Session.Clear();
        return RedirectToPage("/User");
    }
}
