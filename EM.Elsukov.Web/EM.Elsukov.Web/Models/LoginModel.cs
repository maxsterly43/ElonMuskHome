using System.ComponentModel.DataAnnotations;

namespace EM.Elsukov.Web.Models
{
    public class LoginModel
    {
        [Display(Name = "Имя пользователя")]
        [Required]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}