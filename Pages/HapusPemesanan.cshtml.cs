using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

public class HapusPemesananModel : PageModel
{
    private readonly IConfiguration _config;
    public HapusPemesananModel(IConfiguration config) => _config = config;

    public IActionResult OnGet(int id)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand("DELETE FROM pemesanan WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        return RedirectToPage("/Keranjang");
    }
}
