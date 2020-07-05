using System;
using System.Collections.Generic;
using System.Text;

namespace TrendyolLinkConverter.Core.Exceptions
{
   public class LinkConverterDomainException : Exception
    {
        public LinkConverterDomainException()
        { }

        public LinkConverterDomainException(string message)
            : base(message)
        { }

        public LinkConverterDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
