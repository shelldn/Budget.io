using System.ComponentModel.DataAnnotations;

namespace Budget.Id.Models
{
    public class AccountInfo
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string PasswordConfirmation { get; set; }
    }
}