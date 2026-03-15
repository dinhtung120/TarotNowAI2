using System;
using Npgsql;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

class Program
{
    static void Main()
    {
        string connString = "Host=localhost;Database=tarotweb;Username=tarot_user;Password=tarot_dev_pass";
        
        // Tester@123 hash (đơn giản hóa cho script test)
        string passwordHash = "AQAAAAIAAYagAAAAEP0wzX/kZpxH9iV7FzR/W6YpU+G0S1Xf1v5tO0q9U2V5Xz5Z9Z9Z9Z9Z9Z9Z9Z9Z9A=="; 

        try {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            
            // Xóa nếu tồn tại
            using (var cmd = new NpgsqlCommand("DELETE FROM users WHERE email = 'tester_gold@example.com' OR username = 'tester_gold'", conn)) {
                cmd.ExecuteNonQuery();
            }

            // Tạo user mới với số dư
            string insertSql = @"
                INSERT INTO users (
                    id, email, username, password_hash, display_name, date_of_birth, 
                    role, status, gold_balance, diamond_balance, user_level, user_exp, 
                    is_email_verified, created_at, preferred_language, reader_status
                ) VALUES (
                    gen_random_uuid(), 'tester_gold@example.com', 'tester_gold', @pwd, 'Gold Tester', '1990-01-01',
                    'user', 'active', 1000, 500, 5, 1200,
                    true, NOW(), 'vi', 'pending'
                )";

            using (var cmd = new NpgsqlCommand(insertSql, conn)) {
                cmd.Parameters.AddWithValue("pwd", passwordHash);
                cmd.ExecuteNonQuery();
                Console.WriteLine("CREATE_USER_SUCCESS");
            }

            // Kiểm tra lại
            using (var cmd = new NpgsqlCommand("SELECT gold_balance, diamond_balance FROM users WHERE username = 'tester_gold'", conn))
            using (var reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    Console.WriteLine($"Gold: {reader[0]}, Diamond: {reader[1]}");
                }
            }
        } catch (Exception ex) { Console.WriteLine("ERROR: " + ex.Message); }
    }
}
