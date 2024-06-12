using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.Services
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Sử dụng thuật toán SHA256 để băm mật khẩu
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Băm mật khẩu thành mảng byte
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển đổi mảng byte thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Hàm kiểm tra mật khẩu nhập vào có khớp với mật khẩu đã băm hay không
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Băm mật khẩu nhập vào và so sánh với mật khẩu đã băm
            string hashedInput = HashPassword(password);
            return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}
