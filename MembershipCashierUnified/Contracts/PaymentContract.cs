using MembershipCashierUnified.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace MembershipCashierUnified.Contracts
{
    public class PaymentContract : IHasPayment
    {
        [DataMember(Name = "Payment")]
        private PaymentImplementor _payment;

        [JsonIgnore]
        public IPayment Payment
        {
            get { return (_payment as IPayment); }
            set { var x = new PaymentImplementor(); value.CopyTo(x); _payment = x; }
        }
    }
}
