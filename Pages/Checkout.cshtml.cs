using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using DinkToPdf;
using DinkToPdf.Contracts;

public class CheckoutModel : PageModel
{
    private readonly IConfiguration _config;
    private readonly IConverter _converter;

    public CheckoutModel(IConfiguration config, IConverter converter)
    {
        _config = config;
        _converter = converter;
    }

    [BindProperty]
    public int Id { get; set; }

    public string NamaPemesan { get; set; } = "";
    public string EmailPemesan { get; set; } = "";
    public string Hotel { get; set; } = "";
    public DateTime TanggalCheckin { get; set; }
    public string Harga { get; set; } = "";

    public void OnGet(int id)
    {
        Id = id;

        var connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new MySqlConnection(connStr);
        conn.Open();

        var cmd = new MySqlCommand("SELECT * FROM pemesanan WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            NamaPemesan = reader.GetString("nama_pemesan");
            EmailPemesan = reader.GetString("email_pemesan");
            Hotel = reader.GetString("hotel");
            TanggalCheckin = reader.GetDateTime("tanggal_checkin");
            Harga = reader.GetString("harga");
        }
    }

    public IActionResult OnPost()
    {
        string html = $@"
        <h2 style='text-align:center;'>Bukti Pemesanan Hotel</h2>
        <p><strong>Nama:</strong> {NamaPemesan}</p>
        <p><strong>Email:</strong> {EmailPemesan}</p>
        <p><strong>Hotel:</strong> {Hotel}</p>
        <p><strong>Check-in:</strong> {TanggalCheckin:dd MMMM yyyy}</p>
        <p><strong>Harga:</strong> Rp {Harga}</p>
        <hr />
        <p style='font-style:italic;'>Terima kasih telah memesan melalui Pergi.com!</p>";

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = { PaperSize = PaperKind.A4, Orientation = Orientation.Portrait },
            Objects = { new ObjectSettings() { HtmlContent = html } }
        };

        var pdf = _converter.Convert(doc);
        return File(pdf, "application/pdf", "BuktiPemesanan.pdf");
    }
}
