using System;
using System.Collections.Generic;
using System.Text;
using TrendyolLinkConverter.Core.Exceptions;

namespace TrendyolLinkConverter.Core.Models
{
    public class RequestHistory : BaseEntity
    {
        public string Request { get;  set; }
        public string Response { get;  set; }

        public RequestHistory(string request, string response)
        {
            if (!string.IsNullOrEmpty(request))
            {
                Request = request;
            }
            else
            {
                throw new LinkConverterDomainException("Property cannot be null or empty");
            }
            if (!string.IsNullOrEmpty(response))
            {
                Response = response;
            }
            else
            {
                throw new LinkConverterDomainException("Property cannot be null or empty");
            }

        }





    }
}
