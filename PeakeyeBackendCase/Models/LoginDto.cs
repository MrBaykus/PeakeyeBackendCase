using System.ComponentModel.DataAnnotations;

namespace PeakeyeBackendCase.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
    }
}
