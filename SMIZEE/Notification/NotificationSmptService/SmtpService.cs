using System;
using System.Net.Mail;

namespace SF.Expand.Notification
{
    public class SmtpService : INotification
    {
        private const string cMODULE_NAME = "SmtpService";
        private const string cBASE_NAME = @"http://sf.expand.com/";

        void INotification.Send(string[] baseParams, int requestTimeout, string subject, string messageBody, string destinationTO, string destinationCC, string attachementPatch)
        {
            string[] mailsTO = null;
            string[] mailsCC = null;

            MailMessage mailMessage = new MailMessage();
            try
            {
                mailMessage.Body = messageBody;

                mailMessage.From = new MailAddress(baseParams[2]);

                if (destinationTO != null)
                {
                    mailsTO = destinationTO.Split(';');
                    foreach (string mailTO in mailsTO)
                        if (mailTO!="")
                            mailMessage.To.Add(new MailAddress(mailTO));
                }

                if (destinationCC != null)
                {
                    mailsCC = destinationCC.Split(';');
                    foreach (string mailCC in mailsCC)
                        if (mailCC != "")
                            mailMessage.CC.Add(new MailAddress(mailCC));
                }

                if (attachementPatch != null)
                {
                    Attachment attach = new Attachment(attachementPatch);
                    mailMessage.Attachments.Add(attach);
                }

                mailMessage.Subject = subject;

                SmtpClient smtp = new SmtpClient(baseParams[0], Int32.Parse(baseParams[1]));
                smtp.Credentials = new System.Net.NetworkCredential(baseParams[3], baseParams[4]);

                smtp.EnableSsl = baseParams[5].Equals("EnableSsl");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(mailMessage);

                Logger.LOGGER.Write(Logger.LOGGER.LOGGEREventID.INFORMATION, cMODULE_NAME, new string[] { cBASE_NAME, "Email sent!" });

            }
            catch (Exception exp)
            {
                Logger.LOGGER.Write(Logger.LOGGER.LOGGEREventID.EXCEPTION, cMODULE_NAME, new string[] { cBASE_NAME, exp.Message +  (exp.InnerException==null?"":exp.InnerException.Message) , exp.StackTrace });
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

    }
}
