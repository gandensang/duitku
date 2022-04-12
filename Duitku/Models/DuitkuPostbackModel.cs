using System;
using System.Collections.Generic;
using System.Text;

namespace Duitku
{
    public class DuitkuPostbackModel
    {
        public string MerchantKey { get; set; }
        public string MerchantCode { get; set; }
        public int Amount { get; set; }
        public string MerchantOrderId { get; set; }
        public string ProductDetail { get; set; }
        public string AdditionalParam { get; set; }
        public string PaymentCode { get; set; }
        public string ResultCode { get; set; }
        public string MerchantUserId { get; set; }
        public string Reference { get; set; }
        public string Signature { get; set; }
    }
}
