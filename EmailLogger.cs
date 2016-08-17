using System;
using System.Net.Mail;
using System.Configuration;

namespace Logger {
    public class EmailLogger : IEmailLogger
    {
        private string _appName = "";
        private string _message = "";

        public EmailLogger(string appName)
        {
            _appName = appName;
            _message = "";
        }

        public string Identity() {
            return "EmailLogger";
        }

        public void Debug(string text)
        {
            _message += "DEBUG: " + text + Environment.NewLine;
        }

        public void Info(string text)
        {
            _message += "INFO:  " + text + Environment.NewLine;
        }

        public void Warn(string text)
        {
            _message += "WARN:  " + text + Environment.NewLine;
        }

        public void Error(string text)
        {
            _message += "ERROR: " + text + Environment.NewLine;
        }

        public void Error(string text, Exception ex)
        {
            if (ex.InnerException != null)
            {
                Exception inner = ex.InnerException;

                while (inner.InnerException != null)
                    inner = inner.InnerException;

                Console.WriteLine(inner.Message);
                Error(inner.Message + Environment.NewLine + inner.StackTrace);
            }
            else
            {
                Console.WriteLine(ex.Message);
                Error(text + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void Flush() {
            string mailSender = ConfigurationManager.AppSettings.Get("mailSender").ToString();
            string mailRecipient = ConfigurationManager.AppSettings.Get("mailRecipient").ToString();
            string mailSubject = ConfigurationManager.AppSettings.Get("mailSubject").ToString();

            if (_message == "")
                return;

            SendMessage(mailSubject, _message, mailSender, mailRecipient, "");
            _message = "";
        }

        private void SendMessage(string subject, string messageBody, string fromAddress, string toAddress, string ccAddress) {
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();

            // Set the sender's address
            message.From = new MailAddress(fromAddress);
            message.ReplyToList.Add(fromAddress);
            message.Headers.Add("Reply-to", fromAddress);

            // Allow multiple "To" addresses to be separated by a semi-colon
            if (toAddress.Trim().Length > 0) {
                string[] addresses = toAddress.Split(';');

                foreach (string addr in addresses) {
                    if (addr == "")
                        continue;

                    message.To.Add(new MailAddress(addr));
                }
            }

            // Allow multiple "Cc" addresses to be separated by a semi-colon
            if (ccAddress.Trim().Length > 0) {
                foreach (string addr in ccAddress.Split(';')) {
                    message.CC.Add(new MailAddress(addr));
                }
            }

            // Set the subject and message body text
            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = false;
            client.UseDefaultCredentials = true;

            // Set the SMTP server to be used to send the message
            client.Host = ConfigurationManager.AppSettings.Get("MailServer").ToString();

            // set delivery options
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.Delay | DeliveryNotificationOptions.OnFailure;// | DeliveryNotificationOptions.OnSuccess;

            // Send the e-mail message
            try {
                client.Send(message);
            }
            catch (SmtpFailedRecipientsException ex) {
                Error("SMTPFailedRecipients exception", ex);
            }
            catch (InvalidOperationException ex) {
                Error("Invalid operation exception", ex);
            }
            catch (SmtpException ex) {
                Error("SMTP exception", ex);
            }
            catch (Exception ex) {
                Error("Mail send Exception", ex);
            }
            finally {
                Info("Email Sent");
            }
        }
    }
}
