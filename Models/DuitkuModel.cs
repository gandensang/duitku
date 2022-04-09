using System;
using System.Collections.Generic;

namespace Models
{
    public class DuitkuModel
    {
        public string MerchantCode { get; set; }
        public string MerchantKey { get; set; } 
        public int PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string MerchantOrderId { get; set; }
        public string ProductDetails { get; set; }
        public string CustomerVaName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<DuitkuItem> ItemDetails { get; set; }
        public List<DuitkuCostumerDetail> CustomerDetail { get; set; }
        public string CallbackUrl { get; set; } 
        public string ReturnUrl { get; set; } 
        public string Signature { get; set; }
        public int ExpiryPeriod { get; set; } = 300;
    }
}
