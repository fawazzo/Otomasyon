using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "Tc Kimlik Numaranız gereklidir")]
    public string TcKimlikNumarasi { get; set; }

    [Required(ErrorMessage = "Şifre gereklidir")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
