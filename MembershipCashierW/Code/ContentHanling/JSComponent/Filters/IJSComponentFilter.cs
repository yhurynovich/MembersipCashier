using System;

namespace MembershipCashierW.Code.ContentHanling.JSComponent.Filters
{
    internal interface IJSComponentFilter
    {
        Func<JsFileDesc, bool> Filter { get; }
    }
}
