using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace RGB.Back.Infra
{
	public class EMailHelper
	{
		//private string senderEmail = "g01.webapp@gmail.com"; // 寄件者
		private string senderEmail = "rizzgamingbase@gmail.com"; // 寄件者

		public void SendForgetPasswordEmail(string url, string name, string email)
		{
			var subject = "[重設密碼通知]";
			var body = $@"Hi {name},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>我要重設密碼</a>], 以進行重設密碼, 如果您沒有提出申請, 請忽略本信, 謝謝";

			var from = senderEmail;
			var to = email;

			SendViaGoogle(from, to, subject, body);
		}

		public void SendConfirmRegisterEmail(string url, string name, string email)
		{
			var subject = "[新會員確認信]";
			var body = $@"Hi {name},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>的確是我申請會員</a>], 如果您沒有提出申請, 請忽略本信, 謝謝";

			var from = senderEmail;
			var to = email;

			SendViaGoogle(from, to, subject, body);

		}

		public void SendNoPasswordLoginEmail(string url, string name, string email)
		{
			var subject = "[免密碼登入]";
			var body = $@"Hi {name},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>進行登入</a>], 如果您沒有提出申請, 請忽略本信, 謝謝";

			var from = senderEmail;
			var to = email;

			SendViaGoogle(from, to, subject, body);

		}


		public virtual void SendViaGoogle(string from, string to, string subject, string body)
		{
			// 讀取 Gmail 帳號和應用程式密碼
			IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
			var gmailUsername = Config.GetSection("GmailUsername").Value;
				//System.Configuration.ConfigurationManager.AppSettings["GmailUsername"];
			var gmailAppPassword = Config.GetSection("GmailAppPassword").Value;
			//System.Configuration.ConfigurationManager.AppSettings["GmailAppPassword"];

			// 檢查是否有設定
			if (string.IsNullOrEmpty(gmailUsername) || string.IsNullOrEmpty(gmailAppPassword))
			{
				throw new ConfigurationErrorsException("GmailUsername or GmailAppPassword is missing in configuration.");
			}

			// Gmail SMTP 設定
			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(gmailUsername, gmailAppPassword),
				EnableSsl = true,
			};

			// 來自和收件者的電子郵件地址
			var fromAddress = new MailAddress(from);
			var toAddress = new MailAddress(to);

			// 建立郵件物件
			var mailMessage = new MailMessage(fromAddress, toAddress)
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = true,
			};

			// 送出郵件
			smtpClient.Send(mailMessage);
		}



		private void CreateTextFile(string path, string from, string to, string subject, string body)
		{
			var fileName = $"{to.Replace("@", "_")} {DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";

			// 文字檔案的完整路徑
			var fullPath = Path.Combine(path, fileName);

			// 檔案內容
			var contents = $@"from:{from}
to:{to}
subject:{subject}

{body}";

			// 建立文字檔案, 採用UTF8編碼
			File.WriteAllText(fullPath, contents, Encoding.UTF8);
		}
	}
}
