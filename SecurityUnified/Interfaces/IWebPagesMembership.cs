using System;
using System.Runtime.Serialization;

namespace SecurityUnified.Interfaces
{
    public interface IWebPagesMembership: IHasUserId
    {
        [DataMember]
        Nullable<System.DateTime> CreateDate { get; set; }
        [DataMember]
        string ConfirmationToken { get; set; }
        [DataMember]
        Nullable<bool> IsConfirmed { get; set; }
        [DataMember]
        Nullable<System.DateTime> LastPasswordFailureDate { get; set; }
        [DataMember]
        int PasswordFailuresSinceLastSuccess { get; set; }
        [DataMember]
        string Password { get; set; }
        [DataMember]
        Nullable<System.DateTime> PasswordChangedDate { get; set; }
        [DataMember]
        string PasswordSalt { get; set; }
        [DataMember]
        string PasswordVerificationToken { get; set; }
        [DataMember]
        Nullable<System.DateTime> PasswordVerificationTokenExpirationDate { get; set; }
        [DataMember]
        Nullable<int> PasswordChangedBy { get; set; }
    }
}
