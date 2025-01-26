using System.Net;
using System.Net.Mail;

namespace Taiwan_Goodtea.api
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // SMTP 設定
            string smtpServer = "smtp.gmail.com"; // SMTP 主機
            int smtpPort = 587;                  // 埠號 (Gmail 通常是 587)
            string fromEmail = "###@gmail.com"; // 發件人信箱
            string fromPassword = "###"; // 發件人密碼

            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true 
                };

                mail.To.Add(toEmail); 

                using (var smtp = new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(fromEmail, fromPassword); 
                    smtp.EnableSsl = true; 

                    await smtp.SendMailAsync(mail); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"無法傳送郵件: {ex.Message}");
            }
        }
    }
}
