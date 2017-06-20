using SecurityUnified.Exceptions;
using System;
using System.Runtime.Serialization;
using System.Security;

namespace MembershipCashierDL.Code.Exceptions
{
    public class ExistingAddressException : Xxception
    {
        public ExistingAddressException() : base() { }

        public ExistingAddressException(string message) : base(message) { }

        public ExistingAddressException(string message, Exception innerException) : base(message, innerException) { }

        [SecuritySafeCritical]
        protected ExistingAddressException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
