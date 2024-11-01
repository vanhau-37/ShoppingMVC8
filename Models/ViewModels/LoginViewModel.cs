using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Shopping_mvc8.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password), Required]
        public string Password { get; set; }
        public string ReturnURL { get; set; }
    }
}
