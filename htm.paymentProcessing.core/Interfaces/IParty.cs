namespace htm.paymentProcessing.core.Interfaces
{
    public interface IParty : IHasEmailAddress, IHasPhoneNumber
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
