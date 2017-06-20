using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SecurityUnified
{
    public static class Utils
    {

        public static void Retry(int timesToRetry, int delayInMs, IEnumerable<Type> retryCondition, Action code)
        {
            if (delayInMs < 15)
                delayInMs = 15;

            bool success = false;
            int attempts = timesToRetry;
            do
            {
                try
                {
                    code.Invoke();
                    success = true;
                }
                catch (Exception ex)
                {
                    if (attempts < 0)
                        throw ex;
                    if(retryCondition==null || !retryCondition.Any())
                        throw ex;
                    if (!retryCondition.Any(e=>e.IsInstanceOfType(ex)))
                        throw ex;
                    Thread.Sleep(delayInMs);
                }
                attempts--;
            } while (!success && attempts >= 0);
        }
    }
}
