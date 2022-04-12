using System;
using System.Collections.Generic;
using System.Text;

namespace Duitku
{
    public class DuitkuSuccessResponse
    {
        public string MerchantCode { get; set; }
        public string Reference { get; set; }
        public string PaymentUrl { get; set; }
        public string VaNumber { get; set; }
        public string Amount { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}
