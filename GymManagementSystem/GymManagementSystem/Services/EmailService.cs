using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService()
    {
        _apiKey = ConfigurationManager.AppSettings["SendGrid:ApiKey"];
        _fromEmail = ConfigurationManager.AppSettings["SendGrid:FromEmail"];
        _fromName = ConfigurationManager.AppSettings["SendGrid:FromName"];

        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new ArgumentNullException("SendGrid:ApiKey is missing in Web.config.");
        }
    }

    /// <summary>
    /// Gửi một email đơn giản.
    /// </summary>
    /// <param name="toEmail">Email của người nhận.</param>
    /// <param name="toName">Tên của người nhận.</param>
    /// <param name="subject">Tiêu đề email.</param>
    /// <param name="plainTextContent">Nội dung dạng văn bản thuần.</param>
    /// <param name="htmlContent">Nội dung dạng HTML (đẹp hơn).</param>
    /// <returns>Task biểu thị hoạt động gửi email.</returns>
    public async Task SendEmailAsync(string toEmail, string toName, string subject, string plainTextContent, string htmlContent)
    {
        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail, toName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            // Gửi email
            var response = await client.SendEmailAsync(msg);

            // (Tùy chọn) Kiểm tra và ghi log kết quả
            if (!response.IsSuccessStatusCode)
            {
                // Ghi log lỗi để debug
                System.Diagnostics.Debug.WriteLine($"Failed to send email: {response.StatusCode}");
                var responseBody = await response.Body.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Response Body: {responseBody}");
            }
        }
        catch (Exception ex)
        {
            // Ghi log lỗi hệ thống
            System.Diagnostics.Debug.WriteLine($"Exception when sending email: {ex.Message}");
        }
    }
}