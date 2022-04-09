using Commons;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Duitku
{
    public class DuitkuPay
    {
        private readonly string _merchantCode;
        private readonly string _merchantKey;
        private readonly string _callbackUrl;
        private readonly string _returnUrl;
        private readonly int _minuteExpired;
        private readonly string _requestUrl;
        public DuitkuPay(string merchantCode, string merchantKey, string callbackUrl,string returnUrl, bool isDemo = true, int minuteExpired = 300)
        {
            _merchantCode = merchantCode;
            _merchantKey = merchantKey;
            _callbackUrl = callbackUrl;
            _returnUrl = returnUrl;
            _minuteExpired = minuteExpired;

            if (isDemo)
            {
                _requestUrl = "https://sandbox.duitku.com/webapi/api/merchant/v2/inquiry";
            }
            else
            {
                // production
                _requestUrl = "https://passport.duitku.com/webapi/api/merchant/transactionStatus";
            }
        }

        public async Task<ServiceResponse<DuitkuSuccessResponse>> MakePayment(DuitkuOrderModel order)
        {
            var response = new ServiceResponse<DuitkuSuccessResponse>();
            try
            {
                var dataOrder = CreateData(order);
                var parameter = CreateParameter(dataOrder);
                using (var client = new HttpClient())
                {
                    var getResponse = await client.PostAsync(_requestUrl, parameter);
                    if (getResponse.IsSuccessStatusCode)
                    {
                        response.Success = false;
                        response.Message = getResponse.ReasonPhrase;
                        return response;
                    }


                    var content = await getResponse.Content.ReadAsStringAsync();

                    // coba parsing json Data

                    DuitkuSuccessResponse dataJson;

                    try
                    {
                        dataJson = JsonConvert.DeserializeObject<DuitkuSuccessResponse>(content);
                    }
                    catch (Exception err)
                    {
                        if (content.Contains("Payment amount must be equal to all item price"))
                        {
                            response.Success = false;
                            response.Message = "Payment amount must be equal to all item price";
                            return response;
                        }
                        response.Success = false;
                        response.Message = err.Message;
                        return response;
                    }


                    response.Data = dataJson;
                }
            }
            catch (Exception err)
            {
                response.Success = false;
                response.Message = err.Message;
                
            }
            return response;
        }
        DuitkuModel CreateData(DuitkuOrderModel detailOrder)
        {

            var costumerDetail = new List<DuitkuCostumerDetail> 
            {
                new DuitkuCostumerDetail 
                {
                    FirstName = detailOrder.FirstName,
                    LastName = detailOrder.LastName,
                    PhoneNumber = detailOrder.Phone,
                    Email = detailOrder.Email,
                    BillingAddress = new DuitkuCustomerAddress
                    {
                        FirstName = detailOrder.FirstName,
                        LastName=detailOrder.LastName,
                        City = detailOrder.City,
                        Phone = detailOrder.Phone,
                        PostalCode = detailOrder.PostalCode,
                        Address = detailOrder.Address,
                    },
                    ShippingAddress = new DuitkuCustomerAddress
                    {
                        FirstName = detailOrder.FirstName,
                        LastName = detailOrder.LastName,
                        City = detailOrder.City,
                        Phone=detailOrder.Phone,
                        PostalCode=detailOrder.PostalCode,
                        Address = detailOrder.Address,
                    }
                } 
            };

            var dataOrder = new DuitkuModel
            {
                MerchantCode = _merchantCode,
                MerchantKey = _merchantKey,
                PaymentAmount = detailOrder.PaymentAmount,
                PaymentMethod = detailOrder.PaymentMethod,
                // merchandOrderId saat buat parameterAja sekalian buat signature
                ProductDetails = detailOrder.DetailOrder,
                CustomerVaName = detailOrder.CustomerVaName,
                Email = detailOrder.Email,
                PhoneNumber = detailOrder.Phone,
                CustomerDetail = costumerDetail,
                CallbackUrl = _callbackUrl,
                ReturnUrl = _returnUrl,
                ExpiryPeriod = _minuteExpired
            };

            return dataOrder;
        }
        private string CreateSignature(string stringData)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(stringData);
            byte[] hashedBytes = MD5.Create().ComputeHash(asciiBytes);
            string hashResult = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashResult;
        }

        StringContent CreateParameter(DuitkuModel dataDuitku)
        {
            string merchantOrderId = Guid.NewGuid().ToString();
            
            var signature = CreateSignature($"{_merchantCode}{merchantOrderId}{dataDuitku.PaymentAmount}{_merchantKey}");
            dataDuitku.Signature = signature;
            dataDuitku.MerchantOrderId = merchantOrderId;


            var jsonValue = JsonConvert.SerializeObject(dataDuitku);
            var param = new StringContent(jsonValue, Encoding.UTF8, "application/json");

            return param;
        }
    }
}
