using System.Net;
using System.Net.Mail;

namespace QuizApplication.HelperClasses
{
    public class OTPManipulation
    {
        /// <summary>
        /// Generate a random 6-digit OTP
        /// </summary>
        /// <returns></returns>
        public string GenerateOtp()
        {
            Random random = new Random();
            int otpValue = random.Next(100000, 999999);
            return otpValue.ToString();
        }
        /// <summary>
        /// Send an OTP (One-Time Password) via email to the specified recipient.
        /// </summary>
        /// <param name="toEmail">Recipient's email address</param>
        /// <param name="otp">Generated OTP to send</param>
        public void SendOTPEmail(string toEmail, string otp)
        {
            // Create a new MailMessage for sending OTP email
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("brainbuzz2023@outlook.com", "BrainBuzz");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "One-Time Password (OTP) for Password Reset";
            mailMessage.Body = $"Dear {toEmail},\n\nYour OTP for password reset is: {otp}\n\nThis OTP is valid for 5 minutes.\n\nSincerely,\nTeam BrainBuzz";

            // Configure the SMTP client to send the email
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("brainbuzz2023@outlook.com", "MVCquiz2023");

            //Send the OTP mail
            smtpClient.Send(mailMessage);
        }
    }
}
