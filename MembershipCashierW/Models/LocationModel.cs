using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MembershipCashierW.Models
{
    [Serializable]
    public class LocationModel : LocationImplementor
    {
        [XmlIgnore]
        private IEnumerable<IOwner> owners;
        [XmlIgnore]
        public IEnumerable<IOwner> Owners
        {
            get
            {
                if (owners == null)
                    owners = this.GetOwners();
                return owners;
            }
        }
    }
}