using System.Text;
using MimeKit;

namespace NotificationTester.WhoreTemplates;

public class PublicationMessageCreator : IMessageCreator
{
    //string email, string subject, string[] comments, bool isApproved
    public string Email { get; set; }
    public string Subject { get; set; }
    public string[] Comments { get; set; }
    public bool IsApproved { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    
    private readonly SmtpSettings _settings;

    public PublicationMessageCreator(string email, string subject, string[] comments, bool isApproved, SmtpSettings settings, string fullName, string description)
    {
        Email = email;
        Subject = subject;
        Comments = comments;
        IsApproved = isApproved;
        _settings = settings;
        FullName = fullName;
        Description = description;
    }
    
    public MimeMessage Create()
    {
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress("НТБ \u25aa Науково-технічна бібліотека", _settings.Email));

        message.To.Add(new MailboxAddress("", Email));

        message.Subject = Subject;

        string commentsToStr = CommentsFormatter(Comments);
        
        var bodyBuilder = new BodyBuilder()
        {
            HtmlBody = IsApproved ? ApprovedBody(FullName, Description) :
                DisapprovedBody(FullName, Description, commentsToStr)
        };

        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }

    string DisapprovedBody(string fullName, string description, string comments)
    {
        return $@"
        <html>
          <body style='font-family: Mulish, sans-serif; font-size: 16px; line-height: 1.5; color: #333;'>
            <p><b>Відхилено</b></p>
            <p>Шановний(на) {fullName}! Модератором бібліотеки <b>відхилено</b> Вашу публікацію <i>""{description}""</i> з наступної причини:</p>
            <p style='margin-left: 20px; color: #d9534f; white-space: pre-wrap;'><b>{comments}</b></p>
            <p>Прохання внести відповідні правки в опис статті в особистому кабінеті Інформаційної системи <a href='https://is.op.edu.ua/' style='color: #337ab7; text-decoration: none;'>https://is.op.edu.ua/</a></p>
            <p>З повагою, Науково-технічна бібліотека ""Одеської політехніки""</p>
          </body>
        </html>";
    }

    string CommentsFormatter(string[] comments)
    {
        var result = new StringBuilder();
        
        if (comments.Length != 0)
        {
            for (int i = 0; i < comments.Length; i++)
            {
                result.AppendLine($"{i + 1}. {comments[i]};{Environment.NewLine}");
            }

            return result.ToString().TrimEnd();
        }
        
        return "";
    }

    string ApprovedBody(string fullName, string description)
    {
        var template = $@"
        <html>
          <body style='font-family: Mulish, sans-serif; font-size: 16px; line-height: 1.5; color: #333;'>
            <p><b>Схвалено</b></p>
            <p>Шановний(на) {fullName}! Інформуємо Вас, що модератором бібліотеки <b>схвалено</b> Вашу публікацію <i>""{description}""</i>.</p>
            <p>З повагою, Науково-технічна бібліотека ""Одеської політехніки""</p>
          </body>
        </html>";
        
        return template;
    }
}