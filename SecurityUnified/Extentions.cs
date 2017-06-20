using System;
using System.Text;

namespace SecurityUnified
{
    public static class Extentions
    {
        public static string ConcatenateTrace(this Exception exp, string newLineSeparator = @"\n")
        {
            try
            {
                var sb = new StringBuilder();
                Exception ex = exp;
                while (ex != null)
                {
                    sb.Append(newLineSeparator);
                    sb.Append(ex.Message);
                    sb.Append(newLineSeparator);
                    sb.Append(ex.StackTrace == null ? string.Empty : ex.StackTrace.Replace("\\n", newLineSeparator));
                    ex = ex.InnerException;
                    if (ex != null)
                    {
                        sb.Append(newLineSeparator);
                        sb.Append("-----------------------------------");
                        sb.Append(newLineSeparator);
                    }
                }
                return sb.ToString(); ;
            }
            catch (Exception cex)
            {
                return cex.Message;
            }
        }
    }
}
