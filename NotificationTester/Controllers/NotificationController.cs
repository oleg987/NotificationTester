using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace NotificationTester.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotificationController : ControllerBase
{
    private readonly PublicationMessageCreator _creator;
    private readonly EmailSender _sender;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(PublicationMessageCreator creator, EmailSender sender, ILogger<NotificationController> logger)
    {
        _creator = creator;
        _sender = sender;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Send(string email, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{DateTime.Now}: Send action called...");
        var message = _creator.Create(email, "test subject", ["test 1", "test 2", "test 3"]);
        _logger.LogInformation($"{DateTime.Now}: Message created...");

        await _sender.SendAsync(message, cancellationToken);
        
        return Ok();
    }
}