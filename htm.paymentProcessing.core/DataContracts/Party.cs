using htm.paymentProcessing.core.Interfaces;
using System.Runtime.Serialization;

namespace htm.paymentProcessing.core.DataContracts
{
    [DataContract]
    public class Party : IParty
    {
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
    }
}
