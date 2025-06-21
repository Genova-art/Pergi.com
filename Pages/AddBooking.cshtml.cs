using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

public class AddBookingModel : PageModel
{
    private readonly IConfiguration _config;
    public AddBookingModel(IConfiguration config) => _config = config;

    [BindProperty] public string Nama { get; set; } = "";
    [BindProperty] public string Email { get; set; } = "";
    [BindProperty] public DateTime CheckIn { get; set; }
    [BindProperty] public string Hotel { get; set; } = "";
    [BindProperty] public string Harga { get; set; } = "";

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Nama) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Hotel))
        {
            return Page();
        }

        var connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand("INSERT INTO pemesanan (nama_pemesan, email_pemesan, tanggal_checkin, hotel, harga, status) VALUES (@Nama, @Email, @CheckIn, @Hotel, @Harga, 'keranjang')", conn);
        cmd.Parameters.AddWithValue("@Nama", Nama);
        cmd.Parameters.AddWithValue("@Email", Email);
        cmd.Parameters.AddWithValue("@CheckIn", CheckIn);
        cmd.Parameters.AddWithValue("@Hotel", Hotel);
        cmd.Parameters.AddWithValue("@Harga", Harga);
        cmd.ExecuteNonQuery();

        return RedirectToPage("/Keranjang");
    }
}
