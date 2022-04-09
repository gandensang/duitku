using Duitku;
using System;
using System.Collections.Generic;

namespace ConsoleTest
{
    class Program
    {
        static  void Main(string[] args)
        {
            var orderModel = new DuitkuOrderModel
            {
                Address = "alamat",
                PaymentAmount = 10000,
                City = "tulungagung",
                CustomerVaName = "budi santoso",
                DetailOrder = "uji cona",
                FirstName = "budi santoso",
                LastName = "Last name",
                PaymentMethod = DuitkuPaymentMethod.M2,
                Phone = "0897843",
                PostalCode = "66281",
                OderList = new List<DuitkuItem>
                        {
                            new DuitkuItem
                            {
                                Name = "produk 1",
                                Price = 10000,
                                Quantity = 1
                            }
                        }
            };


            string merchantCode = "D4108";
            string merchantKey = "d0e998140e0d34a229be1a56b54c05b9";
            string callbackUrl = "https://exrush.com/callback";
            string returnUrl = "https://exrush.com/testing";


            var duitku = new DuitkuGateway(merchantCode, merchantKey, callbackUrl, returnUrl, true);
            var hasil =  duitku.MakePayment(orderModel).GetAwaiter().GetResult();
        }
    }
}
