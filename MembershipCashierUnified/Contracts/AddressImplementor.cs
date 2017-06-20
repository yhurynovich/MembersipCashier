using MembershipCashierUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MembershipCashierUnified.Code;

namespace MembershipCashierUnified.Contracts
{
    [MetadataType(typeof(IAddress))]
    [KnownType(typeof(AddressContract))]
    [ProvinceOrProvinceNameRequired(ErrorMessageResourceName = "FieldIsRequired", ErrorMessageResourceType = typeof(MCUResources))]
    [DataContract]
    public class AddressImplementor : IAddress
    {
        [DataMember]
        public long AddressId { get; set; }
        [StringLength(100)]
        [Required(ErrorMessageResourceName = "FieldIsRequired", ErrorMessageResourceType = typeof(MCUResources))]
        [RegularExpression("[\\w\\d\\s,.'\\-]{1,100}", ErrorMessageResourceName = "InvalidAddress", ErrorMessageResourceType = typeof(MCUResources))]
        [Display(Name = "Address")]
        [DataMember]
        public string Address1 { get; set; }
        [Required(ErrorMessageResourceName = "FieldIsRequired", ErrorMessageResourceType = typeof(MCUResources))]
        [StringLength(9, ErrorMessageResourceName = "PostalCodeTooBig", ErrorMessageResourceType = typeof(MCUResources))]
        [PostalCodeValidatorAttribute(ErrorMessageResourceName = "InvalidPostalCode", ErrorMessageResourceType = typeof(MCUResources))]
        [Display(Name = "PostalCode")]
        [DataMember]
        public string PostalCode { get; set; }
        [StringLength(35)]
        [Required(ErrorMessageResourceName = "FieldIsRequired", ErrorMessageResourceType = typeof(MCUResources))]
        [RegularExpression("[\\w\\d\\s,.'\\-]{1,35}", ErrorMessageResourceName = "InvalidCityName", ErrorMessageResourceType = typeof(MCUResources))]
        [Display(Name = "City")]
        [DataMember]
        public string City { get; set; }
        [StringLength(2)]
        //[Required(ErrorMessageResourceName = "FieldIsRequired", ErrorMessageResourceType = typeof(Resources))]
        [RegularExpression("([A-Z]{2})?", ErrorMessageResourceName = "InvalidProvinceName", ErrorMessageResourceType = typeof(MCUResources))]
        [Display(Name = "Province")]
        [DataMember]
        public string Province { get; set; }
        [MaxLength(50, ErrorMessageResourceName = "InvalidProvinceName", ErrorMessageResourceType = typeof(MCUResources))]
        [Display(Name = "Province Name")]
        [DataMember]
        public string ProvinceName { get; set; }
        [StringLength(2)]
        [Required(ErrorMessageResourceName = "FieldIsRequired", ErrorMessageResourceType = typeof(MCUResources))]
        [RegularExpression("[A-Z]{2}", ErrorMessageResourceName = "InvalidCountryName", ErrorMessageResourceType = typeof(MCUResources))]
        [Display(Name = "Country")]
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public bool? IsResidential { get; set; }
        [DataMember]
        public byte ValidityLevel { get; set; }
        [DataMember]
        public DateTime? ValidatedTime { get; set; }
    }
}
