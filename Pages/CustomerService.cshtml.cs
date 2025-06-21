using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Pergi.com.Pages
{
    public class CustomerServiceModel : PageModel
    {
        private readonly IConfiguration _config;

        public CustomerServiceModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Phone))
            {
                Message = "Semua field wajib diisi.";
                return Page();
            }

            var connStr = _config.GetConnectionString("DefaultConnection");

            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                var query = "INSERT INTO Messages (Name, Email, Phone) VALUES (@Name, @Email, @Phone)";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Phone", Phone);

                cmd.ExecuteNonQuery();
                Message = "Pesan berhasil dikirim!";
            }
            catch (Exception ex)
            {
                Message = "Gagal kirim pesan: " + ex.Message;
            }

            return Page();
        }
    }
}
