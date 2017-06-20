namespace MembershipCashierUnified.Interfaces
{
    public interface IIAddress
    {
        string Address1 { get; set; }
        string PostalCode { get; set; }
        string City { get; set; }
        string Province { get; set; }
        string Country { get; set; }
    }
}
