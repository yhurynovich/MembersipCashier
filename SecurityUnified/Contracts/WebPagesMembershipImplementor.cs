using SecurityUnified.Interfaces;
using System;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    public class WebPagesMembershipImplementor : IWebPagesMembership
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
        [DataMember]
        public string ConfirmationToken { get; set; }
        [DataMember]
        public bool? IsConfirmed { get; set; }
        [DataMember]
        public DateTime? LastPasswordFailureDate { get; set; }
        [DataMember]
        public int PasswordFailuresSinceLastSuccess { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public DateTime? PasswordChangedDate { get; set; }
        [DataMember]
        public string PasswordSalt { get; set; }
        [DataMember]
        public string PasswordVerificationToken { get; set; }
        [DataMember]
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
        [DataMember]
        public Nullable<int> PasswordChangedBy { get; set; }
    }
}
