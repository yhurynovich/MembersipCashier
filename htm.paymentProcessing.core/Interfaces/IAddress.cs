namespace htm.paymentProcessing.core.Interfaces
{
    public interface IAddress
    {
        string Address1 { get; set; }
        string PostalCode { get; set; }
        string City { get; set; }
        string Province { get; set; }
        string Country { get; set; }
    }
}
