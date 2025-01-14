using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace NotificationTester.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotificationController : ControllerBase
{
    private readonly PublicationMessageCreator _creator;
    private readonly EmailSender _sender;

    public NotificationController(PublicationMessageCreator creator, EmailSender sender)
    {
        _creator = creator;
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Send(string email, CancellationToken cancellationToken)
    {
        var message = _creator.Create(email, "test subject", ["test 1", "test 2", "test 3"]);

        await _sender.SendAsync(message, cancellationToken);
        
        return Ok();
    }
}