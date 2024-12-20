using PeakeyeBackendCase.Data;
using PeakeyeBackendCase.Helpers;
using PeakeyeBackendCase.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace PeakeyeBackendCase.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public User Authenticate(LoginDto loginDto)
        {

            Console.WriteLine("Kullanıcı sorgulanıyor...");
            // LoginDto içindeki kullanıcı adı ile veritabanındaki kullanıcıyı bul
            var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null && !PasswordHelper.VerifyPassword(user.Password, loginDto.Password))
            {
                return null;
            } 
            return user;  
        }

        public string GenerateJwtToken(User user)
        {
            return JwtHelper.GenerateToken(user, _configuration);
        }

        public User Register(RegisterDto registerDto)
        {
            var user = new User
            {
                Username = registerDto.Username,
                Password = registerDto.Password,
                Email = registerDto.Email,
                Role = string.IsNullOrEmpty(registerDto.Role) ? "User" : registerDto.Role,
                Otp = ""
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public string GenerateOtp(User user)
        {
            var otp = new Random().Next(100000, 999999).ToString(); 
            user.Otp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10); // Yerel Saat farkı yüzünden Date Time ayarlanmalı
            _context.SaveChanges();
            return otp;
        }

        public void SendOtp(User user, string otp)
        {
            // E-posta ile OTP gönderme
            Console.WriteLine($"Sending OTP to {user.Email}: {otp}");


            var smtpClient = new SmtpClient("smtp.gmail.com") 
            {
                Port = 587,
                Credentials = new NetworkCredential("example@gmail.com", "password"), 
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("example@gmail.com"),
                Subject = "Your OTP Code",
                Body = $"Your OTP code is {otp}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(user.Email); 

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine($"OTP sent to {user.Email}: {otp}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
            }
        }

        public bool VerifyOtp(string username, string otp)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || user.Otp != otp || user.OtpExpiry < DateTime.Now)
            {
                return false;
            }

            user.Otp = "";
            user.OtpExpiry = null;
            _context.SaveChanges();
            return true;
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
