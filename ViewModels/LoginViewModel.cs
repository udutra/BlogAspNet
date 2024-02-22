using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Informe o E-mail!")]
    [EmailAddress(ErrorMessage = "O E-mail é inválido!")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "informe a senha!")]
    public string Password { get; set; }
}