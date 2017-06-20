using System.Collections.Generic;

namespace MembershipCashierW.Code.ContentHanling.JSComponent.Filters
{
    public class JsFileDescComparer : IEqualityComparer<JsFileDesc>
    {
        public bool Equals(JsFileDesc x, JsFileDesc y)
        {
            return x.File == y.File && x.Folder == y.Folder && x.Type == y.Type;
        }

        public int GetHashCode(JsFileDesc obj)
        {
            return obj.FullPath.GetHashCode();
        }
    }
}