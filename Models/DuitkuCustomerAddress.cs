﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class DuitkuCustomerAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; } = "ID";
    }
}
