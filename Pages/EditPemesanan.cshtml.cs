using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

public class EditPemesananModel : PageModel
{
    private readonly IConfiguration _config;

    public EditPemesananModel(IConfiguration config)
    {
        _config = config;
    }

    [BindProperty]
    public int id { get; set; }  // Pastikan pakai huruf kecil karena kamu pakai `@item.id`

    [BindProperty]
    public string Hotel { get; set; } = "";

    [BindProperty]
    public string NamaPemesan { get; set; } = "";

    [BindProperty]
    public string Email { get; set; } = "";

    [BindProperty]
    public DateTime TanggalCheckin { get; set; }

    [BindProperty]
    public string Harga { get; set; } = "";

    public void OnGet(int id)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand("SELECT * FROM pemesanan WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            this.id = id;
            Hotel = reader.GetString("hotel");
            NamaPemesan = reader.GetString("nama_pemesan");
            Email = reader.GetString("email_pemesan");
            TanggalCheckin = reader.GetDateTime("tanggal_checkin");
            Harga = reader.GetString("harga");
        }
    }

    public IActionResult OnPost()
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand(@"
            UPDATE pemesanan 
            SET hotel = @hotel, nama_pemesan = @nama, email_pemesan = @email, tanggal_checkin = @tanggal, harga = @harga 
            WHERE id = @id", conn);

        cmd.Parameters.AddWithValue("@hotel", Hotel);
        cmd.Parameters.AddWithValue("@nama", NamaPemesan);
        cmd.Parameters.AddWithValue("@email", Email);
        cmd.Parameters.AddWithValue("@tanggal", TanggalCheckin);
        cmd.Parameters.AddWithValue("@harga", Harga);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();

        return RedirectToPage("/Keranjang");
    }
}
