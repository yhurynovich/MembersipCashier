using System.Reflection;
using WebMatrix.WebData;

namespace SecurityDL
{
    public static class Extentions
    {
        public static void InitializeWebSecurity()
        {
            var field = typeof(WebSecurity).GetProperty("Initialized");
            field.SetValue(null, true);
        }
    }
}
