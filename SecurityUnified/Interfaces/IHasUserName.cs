using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SecurityUnified.Interfaces
{
    public interface IHasUserName
    {
        [Required]
        [DataMember]
        [RegularExpression(@"[\w\d]{3,}")]
        string UserName { get; set; }
    }
}
