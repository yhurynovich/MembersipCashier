using SecurityUnified.Interfaces;
using System.Runtime.Serialization;

namespace SecurityUnified.Contracts
{
    [KnownType(typeof(WebPagesMembershipImplementor))]
    [KnownType(typeof(WebPagesMembershipContract))]
    public class WebPagesMembershipDiscriminator : DataDiscriminator<IWebPagesMembership>
    {
    }
}
