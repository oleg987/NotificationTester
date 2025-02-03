using MimeKit;

namespace NotificationTester.WhoreTemplates;

public interface IMessageCreator
{
    public MimeMessage Create();
}