using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MiniCarRentalAPI.Services
{
	public class EmailService
	{
		private readonly SendGridClient _client;
		private readonly EmailAddress _address;
		private readonly SendGridMessage _message;
		public EmailService(IOptions<EmailServiceOptions> options)
		{
			_client = new SendGridClient(options.Value.APIKey);
			_address = new EmailAddress("minicarrental@hotmail.com");
			_message = new SendGridMessage()
			{
				From = new EmailAddress("minicarrental@hotmail.com"),
				Subject = "Confirm Your Rental",
				PlainTextContent = "LINK",
			};
		}
		public async void SendConfirmationEmail(int offerID, string email)
		{
			_message.AddTo(email);
			var response = await _client.SendEmailAsync(_message).ConfigureAwait(false);
		}
	}
	public class EmailServiceOptions
	{
		public string APIKey { get; set; } = string.Empty;
	}


}
