using System;
using System.Collections.Generic;
using System.Text;

namespace Duitku
{
    public class DuitkuOrderModel
    {
        public int PaymentAmount { get; set; }
        public DuitkuPaymentMethod PaymentMethod { get; set; }
        public string CustomerVaName { get; set; }
        public string DetailOrder { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public List<DuitkuItem> OderList { get; set; }
    }
}
