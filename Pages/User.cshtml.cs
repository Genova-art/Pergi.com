using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

public class UserModel : PageModel
{
    private readonly IConfiguration _config;
    public UserModel(IConfiguration config) => _config = config;

    [BindProperty] public LoginData LoginInput { get; set; }
    [BindProperty] public RegisterData RegisterInput { get; set; }
    [BindProperty(SupportsGet = true)] public bool IsRegistering { get; set; }

    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }

    public string FormTitle => IsRegistering ? "Daftar Akun" : "Login Akun";
    public string ButtonText => IsRegistering ? "Daftar" : "Login";

    public void OnGet()
    {
        if (Request.Query.ContainsKey("register")) IsRegistering = true;
        if (Request.Query.ContainsKey("login")) IsRegistering = false;
    }

    public IActionResult OnPost()
    {
        var connStr = _config.GetConnectionString("DefaultConnection");

        try
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            if (IsRegistering)
            {
                // Validasi input kosong
                if (string.IsNullOrWhiteSpace(RegisterInput.Email)
                    || string.IsNullOrWhiteSpace(RegisterInput.Password)
                    || string.IsNullOrWhiteSpace(RegisterInput.FullName))
                {
                    ErrorMessage = "Semua field wajib diisi.";
                    return Page();
                }

                // Cek apakah email sudah ada
                var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE Email=@Email", conn);
                checkCmd.Parameters.AddWithValue("@Email", RegisterInput.Email);
                bool exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;

                if (exists)
                {
                    ErrorMessage = "Email sudah terdaftar.";
                    return Page();
                }

                // Insert user baru
                var insertCmd = new MySqlCommand(
                    "INSERT INTO Users (FullName, Email, Password) VALUES (@FullName, @Email, @Password)", conn);
                insertCmd.Parameters.AddWithValue("@FullName", RegisterInput.FullName);
                insertCmd.Parameters.AddWithValue("@Email", RegisterInput.Email);
                insertCmd.Parameters.AddWithValue("@Password", RegisterInput.Password);
                insertCmd.ExecuteNonQuery();

                Message = "Pendaftaran berhasil! Silakan login.";
                IsRegistering = false;
                return Page();
            }
            else
            {
                // Proses login
                if (string.IsNullOrWhiteSpace(LoginInput.Email)
                    || string.IsNullOrWhiteSpace(LoginInput.Password))
                {
                    ErrorMessage = "Email dan password wajib diisi.";
                    return Page();
                }

                var cmd = new MySqlCommand(
                    "SELECT Id, FullName, Email FROM Users WHERE Email=@Email AND Password=@Password", conn);
                cmd.Parameters.AddWithValue("@Email", LoginInput.Email);
                cmd.Parameters.AddWithValue("@Password", LoginInput.Password);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Simpan session setelah login berhasil
                    HttpContext.Session.SetString("UserId", reader["Id"].ToString()!);
                    HttpContext.Session.SetString("FullName", reader["FullName"].ToString()!);
                    HttpContext.Session.SetString("Email", reader["Email"].ToString()!);

                    return RedirectToPage("/Profile");
                }

                ErrorMessage = "Email atau password salah.";
                return Page();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Terjadi kesalahan: " + ex.Message;
            return Page();
        }
    }

    public class LoginData
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterData
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
