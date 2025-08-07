using System;
using System;
using System.Configuration;
using System.Configuration;
using System.Net.Http;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using GymManagementSystem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json;

public class MomoService
{
    private readonly string _partnerCode;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly string _endpoint;
    private readonly string _returnUrl;
    private readonly string _ipnUrl;

    public MomoService()
    {
        _partnerCode = ConfigurationManager.AppSettings["Momo:PartnerCode"];
        _accessKey = ConfigurationManager.AppSettings["Momo:AccessKey"];
        _secretKey = ConfigurationManager.AppSettings["Momo:SecretKey"];
        _endpoint = ConfigurationManager.AppSettings["Momo:Endpoint"];
        _returnUrl = ConfigurationManager.AppSettings["Momo:ReturnUrl"];
        _ipnUrl = ConfigurationManager.AppSettings["Momo:IpnUrl"];
    }

    public async Task<string> CreatePaymentUrlAsync(int hoaDonId, decimal amount, string orderInfo)
    {
        var orderId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();
        var amountString = ((long)amount).ToString();
        var extraData = "";

        var rawSignature = $"accessKey={_accessKey}&amount={amountString}&extraData={extraData}&ipnUrl={_ipnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={_partnerCode}&redirectUrl={_returnUrl}&requestId={requestId}&requestType=captureWallet";
        var signature = CreateHmacSha256(rawSignature, _secretKey);

        var payload = new
        {
            partnerCode = _partnerCode,
            requestId = requestId,
            amount = amountString,
            orderId = orderId,
            orderInfo = orderInfo,
            redirectUrl = _returnUrl,
            ipnUrl = _ipnUrl,
            lang = "vi",
            extraData = extraData,
            requestType = "captureWallet",
            signature = signature
        };

        using (var client = new HttpClient())
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<MomoResponse>(responseString);

            if (responseData?.resultCode == 0)
            {
                // --- THAY ĐỔI QUAN TRỌNG: LƯU CẢ MomoOrderId VÀ PayUrl ---
                using (var db = new ApplicationDbContext())
                {
                    var hoaDon = await db.HoaDons.FindAsync(hoaDonId);
                    if (hoaDon != null)
                    {
                        hoaDon.MomoOrderId = orderId;       // Lưu mã giao dịch để đối chiếu IPN
                        hoaDon.PayUrl = responseData.payUrl; // <-- LƯU URL THANH TOÁN
                        await db.SaveChangesAsync();
                    }
                }
                // ---------------------------------------------------------
                return responseData.payUrl;
            }
            else
            {
                Console.WriteLine($"Lỗi MoMo: {responseData?.message}");
                return null;
            }
        }
    }

    private string CreateHmacSha256(string message, string key)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    // Lớp để deserialize response từ MoMo
    private class MomoResponse
    {
        public string partnerCode { get; set; }
        public string orderId { get; set; }
        public string requestId { get; set; }
        public long amount { get; set; }
        public long responseTime { get; set; }
        public string message { get; set; }
        public int resultCode { get; set; }
        public string payUrl { get; set; }
    }
}