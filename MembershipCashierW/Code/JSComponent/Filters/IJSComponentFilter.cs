using MembershipCashierW.Code.ContentHanling.JSComponent.Filters;
using System;

namespace MembershipCashierW.Code.JSComponent.Filters
{
    internal interface IJSComponentFilter
    {
        Func<JsFileDesc, bool> Filter { get; }
    }
}
