using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using NotificationTester.WhoreTemplates;

public class EmailSender
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<SmtpSettings> settings, ILogger<EmailSender> logger)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public async Task SendAsync(IMessageCreator messageCreator, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient();
        
        _logger.LogInformation($"{DateTime.Now}: SMTP Client created...");
        
        try
        {
            await client.ConnectAsync(_settings.Server, _settings.Port, SecureSocketOptions.SslOnConnect,
                cancellationToken);
            
            _logger.LogInformation($"{DateTime.Now}: Connected -> {client.IsConnected}...");
            
            await client.AuthenticateAsync(_settings.Email, _settings.Password, cancellationToken);
            
            _logger.LogInformation($"{DateTime.Now}: Authenticated -> {client.IsAuthenticated}...");
            
            var response = await client.SendAsync(messageCreator.Create(), cancellationToken);
            
            _logger.LogInformation($"{DateTime.Now}: Message send. Server response: {response}...");
        }
        catch (Exception e)
        {
            _logger.LogError($"{DateTime.Now}: Exception: {e}");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
            _logger.LogInformation($"{DateTime.Now}: Disconnected.");
        }
    }
}