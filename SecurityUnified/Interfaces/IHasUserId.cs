using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SecurityUnified.Interfaces
{
    public interface IHasUserId
    {
        [Required]
        [DataMember]
        int UserId { get; set; }
    }
}
