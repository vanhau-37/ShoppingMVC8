using System.ComponentModel.DataAnnotations;

namespace Shopping_mvc8.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required,EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password),Required]
		public string Password { get; set; }
	}
}
