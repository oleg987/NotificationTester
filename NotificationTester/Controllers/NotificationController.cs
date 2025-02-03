using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NotificationTester.WhoreTemplates;

namespace NotificationTester.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotificationController : ControllerBase
{
    private readonly EmailSender _sender;
    private readonly ILogger<NotificationController> _logger;
    private readonly IOptions<SmtpSettings> _smtpSettings;

    public NotificationController(EmailSender sender, ILogger<NotificationController> logger, IOptions<SmtpSettings> smtpSettings)
    {
        _sender = sender;
        _logger = logger;
        _smtpSettings = smtpSettings;
    }

    [HttpGet]
    public async Task<IActionResult> Send(string email, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{DateTime.Now}: Send action called...");

        var creator = new PublicationMessageCreator(
            email, 
            "Щодо модерації Вашої публікації Науково-технічною бібліотекою",
            ["Неправильна кількість сторінок", "Відсутнє DOI", "Неправильний бібліографічний опис"],
            true,_smtpSettings.Value,
            "Вася Лупкин",
            "Йожаки");
        
        _logger.LogInformation($"{DateTime.Now}: Message created...");
        
        await _sender.SendAsync(creator, cancellationToken);
        
        return Ok();
    }
}