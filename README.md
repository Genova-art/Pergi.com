# 🌍 Pergi.com - Aplikasi Pemesanan Hotel (Projek UAS)

Selamat datang di **Pergi.com**, sebuah aplikasi web pemesanan hotel berbasis **ASP.NET Razor Pages** dan **MySQL**. Projek ini dibuat sebagai **tugas akhir (UAS)** untuk mata kuliah **Pemrograman Web**.

---

## 🎯 Tujuan Aplikasi

- Memberikan pengalaman mudah dalam memesan hotel secara online.
- Menerapkan konsep **CRUD (Create, Read, Update, Delete)** dengan **session login**.
- Menyediakan fitur **checkout** dan **download struk dalam format PDF**.

---

## 🛠 Teknologi yang Digunakan

- ASP.NET Core Razor Pages
- C#
- MySQL (HeidiSQL / phpMyAdmin)
- DinkToPdf (generate PDF)
- HTML, CSS (Custom)
- Session & Middleware

---

## 📌 Fitur Utama

| Fitur                   | Keterangan                                     |
| ----------------------- | ---------------------------------------------- |
| Registrasi & Login      | Pengguna bisa daftar dan login dengan session  |
| Lihat & Edit Profil     | Bisa ubah nama & email akun                    |
| Hapus Akun              | Akun bisa dihapus dari sistem                  |
| Booking Hotel           | Tambah pesanan hotel ke database               |
| Keranjang               | Lihat daftar pesanan hotel yang belum checkout |
| Edit & Hapus Pemesanan  | Ubah/hapus data pemesanan di keranjang         |
| Checkout + Download PDF | Konfirmasi pemesanan dan unduh struk transaksi |

---

## 🧭 Alur Fitur (User Journey)

1. **User Register** → mengisi nama, email, password.
2. **User Login** → data disimpan di `Session`.
3. **Dashboard/Profile** → lihat nama dan email.
4. **User memesan hotel** (input hotel, tanggal, nama/email).
5. **Pemesanan masuk ke "Keranjang"** (status: belum checkout).
6. Di **Keranjang**, user bisa:
   - Lihat semua pemesanan
   - Ubah pesanan
   - Hapus pesanan
   - Checkout
7. Setelah Checkout:
   - Status pemesanan berubah
   - User bisa **download PDF struk**
8. User bisa logout atau hapus akun kapan saja.

---

## 🗃 Struktur Database (MySQL)

### 📄 Tabel `Users`

| Kolom    | Tipe    | Keterangan                  |
| -------- | ------- | --------------------------- |
| Id       | INT     | Primary key, auto-increment |
| FullName | VARCHAR | Nama lengkap pengguna       |
| Email    | VARCHAR | Email unik pengguna         |
| Password | VARCHAR | Password (plaintext demo)   |

### 📄 Tabel `Pemesanan`

| Kolom          | Tipe    | Keterangan                           |
| -------------- | ------- | ------------------------------------ |
| Id             | INT     | Primary key, auto-increment          |
| Id_User        | INT     | Foreign key ke tabel `Users`         |
| Hotel          | VARCHAR | Nama hotel                           |
| NamaPemesan    | VARCHAR | Nama orang yang booking              |
| EmailPemesan   | VARCHAR | Email pemesan                        |
| TanggalCheckin | DATE    | Tanggal check-in                     |
| Harga          | INT     | Harga hotel                          |
| Status         | VARCHAR | Status booking ('belum', 'checkout') |

---

## ⚙️ Cara Menjalankan Aplikasi

1. Clone / download project ini
2. Import database ke MySQL, pastikan nama tabel & field sama
3. Ubah file `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=pergi_db;Uid=root;Pwd=;"
     }
   }
   ```
