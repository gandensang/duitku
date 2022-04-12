using Duitku.commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Duitku
{
    public class DuitkuGateway
    {
        private readonly string _merchantCode;
        private readonly string _merchantKey;
        private readonly string _callbackUrl;
        private readonly string _returnUrl;
        private readonly int _minuteExpired;
        private readonly string _requestUrl;
        public DuitkuGateway(string merchantCode, string merchantKey, string callbackUrl,string returnUrl, bool isDemo = false, int minuteExpired = 300)
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
                _requestUrl = "https://passport.duitku.com/webapi/api/merchant/v2/inquiry";
            }
        }

        public async Task<DuitkuServiceResponse<string>> MakePayment(DuitkuOrderModel order)
        {
            var response = new DuitkuServiceResponse<string>();
            try
            {

                var dataOrder = CreateData(order);
                var parameter = CreateParameter(dataOrder);
                using (var client = new HttpClient())
                {
                    var getResponse = await client.PostAsync(_requestUrl, parameter);
                    if (!getResponse.IsSuccessStatusCode)
                    {
                        response.Success = false;
                        response.Message = getResponse.ReasonPhrase;
                        return response;
                    }

                    var content = await getResponse.Content.ReadAsStringAsync();

                    response.Data = content;
                }
            }
            catch (Exception err)
            {
                response.Success = false;
                response.Message = err.Message;
                
            }
            return response;
        }
        public DuitkuServiceResponse<bool> VerifikasiTransaksi(string merchantCode, int amount, string merchantOrderId, string signature, string merchantKey)
        {
            var response = new DuitkuServiceResponse<bool>();
            try
            {
                // jika parameter ada yang kosong
                if (string.IsNullOrEmpty(merchantCode) || string.IsNullOrEmpty(merchantOrderId) || string.IsNullOrEmpty(merchantKey))
                {
                    response.Success = false;
                    response.Message = "Bad Parameter";
                    return response;
                }

                // jika parameter berar, susun untuk check md5
                var checkSignature = CreateSignature($"{merchantCode}{merchantOrderId}{amount}{merchantKey}");

                // verifikasi md5 dengan versi server
                if (checkSignature != signature)
                {
                    // gagal
                    response.Success = false;
                    response.Message = "Wrong signature";
                    return response;
                }

                response.Data = true;
            }
            catch (Exception err)
            {
                response.Success = false;
                response.Message = err.Message;
                
            }
            return response;
        }

        #region custom function
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
                MerchantOrderId = detailOrder.MerchantOrderId,
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
                ExpiryPeriod = _minuteExpired,
                AdditionalParam = detailOrder.AdditionalParam
            };

            return dataOrder;
        }
        public string CreateSignature(string stringData)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(stringData);
            byte[] hashedBytes = MD5.Create().ComputeHash(asciiBytes);
            string hashResult = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashResult;
        }
        StringContent CreateParameter(DuitkuModel dataDuitku)
        {
            
            var signature = CreateSignature($"{_merchantCode}{dataDuitku.MerchantOrderId}{dataDuitku.PaymentAmount}{_merchantKey}");
            dataDuitku.Signature = signature;


            var jsonValue = JsonConvert.SerializeObject(dataDuitku);
            var param = new StringContent(jsonValue, Encoding.UTF8, "application/json");

            return param;
        }
        #endregion
    }


    #region class model private
    // class pendukung
    class DuitkuCustomerAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; } = "ID";
    }

    class DuitkuModel
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
        public string AdditionalParam { get; set; }
    }

    class DuitkuCostumerDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DuitkuCustomerAddress BillingAddress { get; set; }
        public DuitkuCustomerAddress ShippingAddress { get; set; }
    }
    #endregion
}
