using System.Net;
using System.Net.Mail;

namespace BlogAspNet.Services;

public class EmailService
{
    public bool Send(string toName, string toEmail, string subject, string body, string fromName = "Equipe Guilherme/Way", string fromEmail = "guilherme.dutra@wayamadeus.com.br")
    {
        var smtpCliente = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);
        smtpCliente.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
        smtpCliente.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpCliente.EnableSsl = true;

        var mail = new MailMessage();

        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;
        
        try
        {
            smtpCliente.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }
}