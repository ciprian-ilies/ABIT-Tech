using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmailSenderWebApp
{
    public class Program
    {
        private const string EmailContent = @"<p>Email BODY</p>";

        private const string EmailSubject = "ABIT Technologies";

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            await SendEmail();
        }

        private static async Task SendEmail()
        {
            var client = new SendGridClient("SENDGRID_KEY");

            log.Debug("SendGrid connected successfully!");

            var emailList = ReadEmails();

            log.Debug("List of e-mails imported!");

            for (var i = 0; i < emailList.Count; i++)
            {
                try
                {
                    var msg = new SendGridMessage
                    {
                        From = new EmailAddress("email@domain.com", "Name"),
                        Subject = EmailSubject,
                        HtmlContent = EmailContent
                    };
                    msg.AddTo(new EmailAddress(emailList[i]));
                    await client.SendEmailAsync(msg);

                    log.Debug($"{i} {emailList[i]} successfully send!");
                    Thread.Sleep(GetWaitingTime());
                }
                catch (Exception e)
                {
                    log.Error($"{i} {emailList[i]} was not send!", e);
                }
            }
        }

        //send e-mails between 10-15 minutes
        private static int GetWaitingTime()
        {
            var random = new Random();
            var time = random.Next(6, 9) * 100000;
            return time;
        }

        //get the e-mails from a csv
        private static List<string> ReadEmails()
        {
            var emailList = new List<string>();
            using (var reader = new StreamReader(@"emails.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    emailList.Add(line);
                }
            }

            return emailList;
        }
    }
}
