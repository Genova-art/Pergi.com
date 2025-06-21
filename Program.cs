using DinkToPdf;
using DinkToPdf.Contracts;
using System.Runtime.Loader;

var builder = WebApplication.CreateBuilder(args);

// ✅ Tambahkan Razor Pages dan Session
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Load library wkhtmltox.dll untuk DinkToPdf
var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

// ✅ Daftarkan DinkToPdf converter sebagai service
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// ✅ Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Harus sebelum Authorization
app.UseAuthorization();

app.MapRazorPages().WithStaticAssets();

app.Run();
