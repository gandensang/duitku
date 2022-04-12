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
                MerchantOrderId = Guid.NewGuid().ToString(),
                Address = "alamat",
                PaymentAmount = 10000,
                City = "tulungagung",
                CustomerVaName = "budi santoso",
                DetailOrder = "uji cona",
                FirstName = "budi santoso",
                LastName = "Last name",
                PaymentMethod = DuitkuPaymentMethod.M2.ToString(),
                Phone = "0897843",
                PostalCode = "66281",
                AdditionalParam = "Other param",
                OderLists = new List<DuitkuItem>
                        {
                            new DuitkuItem
                            {
                                Name = "produk 1",
                                TotalPrice = 10000,
                                Quantity = 1
                            }
                        }
            };


            string merchantCode = "D4108";
            string merchantKey = "d0e998140e0d34a229be1a56b54c05b9";
            string callbackUrl = "https://exrush.com/callback";
            string returnUrl = "https://exrush.com/testing";
            string orderId = "3b0ca6f4-6bd0-439c-8d99-4f5cf1961864";
            int amount = 10000;

            var duitku = new DuitkuGateway(merchantCode, merchantKey, callbackUrl, returnUrl, true);
            // var hasil =  duitku.MakePayment(orderModel).GetAwaiter().GetResult();

            var getCode = duitku.CreateSignature($"{merchantCode}{orderId}{amount}{merchantKey}");
        }
    }
}
