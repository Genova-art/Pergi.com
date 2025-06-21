using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

public class BookingListModel : PageModel
{
    private readonly IConfiguration _config;
    public List<Pemesanan> Bookings = new();

    public BookingListModel(IConfiguration config)
    {
        _config = config;
    }

    public void OnGet()
    {
        string connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand("SELECT * FROM pemesanan", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Bookings.Add(new Pemesanan
            {
                Id = reader.GetInt32("id"),
                Nama = reader.GetString("nama_pemesan"),
                Email = reader.GetString("email_pemesan"),
                CheckIn = reader.GetDateTime("tanggal_checkin"),
                Hotel = reader.GetString("hotel"),
                Harga = reader.GetString("harga")
            });
        }
    }

    public class Pemesanan
    {
        public int Id { get; set; }
        public string Nama { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime CheckIn { get; set; }
        public string Hotel { get; set; } = "";
        public string Harga { get; set; } = "";
    }
}
