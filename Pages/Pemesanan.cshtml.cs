using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

public class PemesananModel : PageModel
{
    private readonly IConfiguration _configuration;

    public PemesananModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string HotelName { get; set; } = string.Empty;
    public string Harga { get; set; } = string.Empty;

    [BindProperty]
    public string? NamaPemesan { get; set; }

    [BindProperty]
    public string? EmailPemesan { get; set; }

    [BindProperty]
    public string? TanggalCheckIn { get; set; }

    public void OnGet(string hotel, string harga)
    {
        HotelName = hotel;
        Harga = harga;
    }

    public IActionResult OnPost()
    {
        System.Diagnostics.Debug.WriteLine("✅ OnPost dijalankan");
        System.Diagnostics.Debug.WriteLine("Nama: " + NamaPemesan);
        System.Diagnostics.Debug.WriteLine("Hotel: " + HotelName);
        System.Diagnostics.Debug.WriteLine("Harga: " + Harga);

        if (string.IsNullOrEmpty(NamaPemesan) ||
            string.IsNullOrEmpty(EmailPemesan) ||
            string.IsNullOrEmpty(TanggalCheckIn) ||
            string.IsNullOrEmpty(Request.Query["hotel"]) ||
            string.IsNullOrEmpty(Request.Query["harga"]))
        {
            ViewData["Error"] = "❗ Semua field harus diisi.";
            return Page();
        }

        HotelName = Request.Query["hotel"];
        Harga = Request.Query["harga"];

        try
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"INSERT INTO pemesanan 
                                (nama_pemesan, email_pemesan, tanggal_checkin, hotel, harga) 
                                VALUES (@nama, @email, @tanggal, @hotel, @harga)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", NamaPemesan);
                    cmd.Parameters.AddWithValue("@email", EmailPemesan);
                    cmd.Parameters.AddWithValue("@tanggal", TanggalCheckIn);
                    cmd.Parameters.AddWithValue("@hotel", HotelName);
                    cmd.Parameters.AddWithValue("@harga", Harga);

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToPage("TerimaKasih");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("❌ ERROR: " + ex.Message);
            ViewData["Error"] = "Gagal menyimpan: " + ex.Message;
            return Page();
        }
    }
}
