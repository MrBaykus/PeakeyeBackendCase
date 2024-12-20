using System.ComponentModel.DataAnnotations;

namespace PeakeyeBackendCase.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
        [RegularExpression("^(User|Admin)$", ErrorMessage = "Role sadece 'Admin' veya 'User' olabilir.")]
        public string Role { get; set; }
    }
}
