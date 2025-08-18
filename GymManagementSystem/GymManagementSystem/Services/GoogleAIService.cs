// Trong file Services/GoogleAIService.cs
using System;
using System.Collections.Generic; // Thêm using này
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using Google.Protobuf.WellKnownTypes; // Thêm using này
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using Newtonsoft.Json;

using GymManagementSystem.Models; // Cần cho ChiSoSucKhoe
using System.Linq; // Cần cho .FirstOrDefault()
using GymManagementSystem.Models.ViewModels; // Cần cho các lớp GeminiResponse


namespace GymManagementSystem.Services
{
    public class GoogleAiService
    {
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly HttpClient _httpClient;

        public GoogleAiService()
        {
            _apiKey = ConfigurationManager.AppSettings["GoogleAI:ApiKey"];
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new ArgumentNullException("GoogleAiApiKey is missing in Web.config.");
            }

            _apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";
            _httpClient = new HttpClient();
        }

        public async Task<string> GenerateHealthAdviceAsync(ChiSoSucKhoe chiSo, HoiVien hoiVien)
        {
            try
            {
                // Truyền cả phân loại BMI và mục tiêu vào prompt
                double chieuCaoM = hoiVien.ChieuCao / 100.0;
                double bmi = Math.Round(chiSo.CanNang / (chieuCaoM * chieuCaoM), 2);
                string phanLoaiBMI = GetPhanLoaiBMI(bmi);

                string prompt = $@"Bạn là một chuyên gia dinh dưỡng và huấn luyện viên cá nhân. Dựa trên các thông tin sau của một hội viên:
                    - Chỉ số BMI: {bmi:F2}
                    - Phân loại: {phanLoaiBMI}
                    - Mục tiêu tập luyện: '{hoiVien.MucTieuTapLuyen}'
                    Hãy viết từ 2 đến 3 dòng đưa ra lời khuyên về: chế độ ăn uống, chế độ luyện tập. Bắt đầu mỗi ý bằng dấu chấm to đầu dòng (•). Giọng văn cần tích cực và khích lệ.";

                var payload = new
                {
                    contents = new[] { new { parts = new[] { new { text = prompt } } } }
                };

                return await SendRequestToGeminiApi(payload);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in GenerateHealthAdviceAsync: {ex.Message}");
                return "Đã có lỗi xảy ra khi tạo lời khuyên.";
            }
        }

        // === PHƯƠNG THỨC CHATBOT (SỬ DỤNG REST API) ===
        public async Task<string> ConverseAsync(string userMessage, List<GeminiContent> conversationHistory, ChiSoSucKhoe latestHealthIndex, HoiVien hoiVien) 
        {
            try
            {
                double chieuCaoM = hoiVien.ChieuCao / 100.0;
                double bmi = Math.Round(latestHealthIndex.CanNang / (chieuCaoM * chieuCaoM), 2);
                string phanLoaiBMI = GetPhanLoaiBMI(bmi);

                string systemContext = $@"Bạn là FitBot, một trợ lý ảo chuyên gia về sức khỏe và gym. Bạn đang trò chuyện với một hội viên có các chỉ số sức khỏe mới nhất như sau:
                    - Chỉ số BMI: {bmi:F2}
                    - Phân loại: {phanLoaiBMI}
                    - Lời khuyên ban đầu đã đưa ra: '{latestHealthIndex.LoiKhuyenAI}'
                    - Mục tiêu của họ: '{hoiVien.MucTieuTapLuyen}'
                    Nhiệm vụ của bạn là trả lời ngắn gọn, tích cực, không đưa ra lời khuyên y tế thay thế bác sĩ.";

                if (conversationHistory.Count == 0)
                {
                    conversationHistory.Add(new GeminiContent { Role = "user", Parts = new List<GeminiPart> { new GeminiPart { Text = systemContext } } });
                    conversationHistory.Add(new GeminiContent { Role = "model", Parts = new List<GeminiPart> { new GeminiPart { Text = "Chào bạn, tôi là FitBot. Tôi có thể giúp gì cho bạn dựa trên các chỉ số sức khỏe hiện tại?" } } });
                }

                conversationHistory.Add(new GeminiContent { Role = "user", Parts = new List<GeminiPart> { new GeminiPart { Text = userMessage } } });

                var payload = new { contents = conversationHistory };
                string botResponseText = await SendRequestToGeminiApi(payload);
                conversationHistory.Add(new GeminiContent { Role = "model", Parts = new List<GeminiPart> { new GeminiPart { Text = botResponseText } } });

                return botResponseText;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in ConverseAsync: {ex.Message}");
                return "Xin lỗi, tôi đang gặp sự cố kỹ thuật.";
            }
        }

        // === HÀM GỬI REQUEST DÙNG CHUNG ===
        private async Task<string> SendRequestToGeminiApi(object payload)
        {
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<GeminiResponse>(responseString);
                return responseData?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text?.Trim() ?? "Tôi chưa có câu trả lời cho vấn đề này.";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Google AI API Error: {response.StatusCode} - {errorContent}");
                return "Không thể kết nối đến dịch vụ AI do lỗi từ API.";
            }
        }

        private string GetPhanLoaiBMI(double bmi)
        {
            if (bmi < 18.5) return "Gầy";
            if (bmi < 25) return "Bình thường";
            if (bmi < 30) return "Thừa cân";
            return "Béo phì";
        }
    }
}