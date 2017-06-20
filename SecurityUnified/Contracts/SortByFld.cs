using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [DataContract]
    [KnownType(typeof(DataDiscriminator<>))]
    public class SortByFld
    {
        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public bool IsDescending { get; set; }
    }
}
