using SecurityUnified.Contracts;
using System.Runtime.Serialization;

namespace MembershipCashierW.Models
{
    [DataContract]
    public class LoginModel : LoginImplementor
    {
        [DataMember]
        public string Captcha { get; set; }
        [DataMember]
        public string LdapAccount { get; set; }
    }
}