using System.ComponentModel.DataAnnotations;
namespace SecurityUnified.Interfaces
{
   public interface ILogin : IHasUserName
    {
        [Required]
        [RegularExpression(@"[^\s]{6,}")]
        string Password { get; set; }

        bool RememberMe { get; set; }

        int RetryCount { get; set; }
    }
}
