using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

public class KeranjangModel : PageModel
{
    private readonly IConfiguration _config;

    public KeranjangModel(IConfiguration config)
    {
        _config = config;
    }

    public List<Pemesanan> ListPemesanan { get; set; } = new();

    public void OnGet()
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand("SELECT * FROM pemesanan", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            ListPemesanan.Add(new Pemesanan
            {
                id = reader.GetInt32("id"), // pakai huruf kecil
                Hotel = reader.GetString("hotel"),
                NamaPemesan = reader.GetString("nama_pemesan"),
                Email = reader.GetString("email_pemesan"),
                TanggalCheckin = reader.GetDateTime("tanggal_checkin"),
                Harga = reader.GetString("harga")
            });
        }
    }

    public class Pemesanan
    {
        public int id { get; set; }
        public string Hotel { get; set; }
        public string NamaPemesan { get; set; }
        public string Email { get; set; }
        public DateTime TanggalCheckin { get; set; }
        public string Harga { get; set; }
    }
}
