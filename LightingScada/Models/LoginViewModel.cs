using System.ComponentModel.DataAnnotations;

namespace LightingScada.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Вкажіть логін")]
    [Display(Name = "Логін")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "Вкажіть пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
}
