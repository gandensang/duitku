using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class DuitkuCostumerDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DuitkuCustomerAddress BillingAddress { get; set; }
        public DuitkuCustomerAddress ShippingAddress { get; set; }
    }
}
