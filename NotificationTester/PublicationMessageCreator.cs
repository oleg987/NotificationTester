using System.Text;
using Microsoft.Extensions.Options;
using MimeKit;

public class PublicationMessageCreator
{
    private readonly SmtpSettings _settings;

    public PublicationMessageCreator(IOptions<SmtpSettings> settings)
    {
        _settings = settings.Value;
    }

    public MimeMessage Create(string email, string subject, string[] titles)
    {
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress("НТБ \u25aa Науково-технічна бібліотека", _settings.Email));

        message.To.Add(new MailboxAddress("", email));

        message.Subject = subject;

        string listOfPublication = TitleFormatter(titles);

        var html =
            $@"
    <html>
      <body style='font-family: Mulish, sans-serif; font-size: 16px; line-height: 1.5;'>
        <p>Доброго дня!</p>
        <p>Модератором бібліотеки були відхилені такі публікації:</p>
        <div style='margin-left: 20px; color: #d9534f; white-space: pre-wrap;'>{listOfPublication}</div>
        <p>З повагою,</p>
        <p><b>Науково-технічна бібліотека</b>!</p>
      </body>
    </html>";

        var bodyBuilder = new BodyBuilder()
        {
            HtmlBody = html
        };

        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }
    
    string TitleFormatter(string[] titles)
    {
        var result = new StringBuilder();
        
        if (titles.Length != 0)
        {
            for (int i = 0; i < titles.Length; i++)
            {
                result.AppendLine($"{i + 1}. {titles[i]};{Environment.NewLine}");
            }

            return result.ToString().TrimEnd();
        }
        
        return "";
    }
}