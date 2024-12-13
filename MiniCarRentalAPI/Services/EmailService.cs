using Humanizer;
using Microsoft.Extensions.Options;
using MiniCarRentalAPI.Controllers;
using SendGrid;
using SendGrid.Helpers.Mail;
using SharedDataModels;
using System.Drawing.Drawing2D;

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
		}
		public async void SendConfirmationEmail(Offer offer, Car car)
		{
			string templateId = "d-e2f6c8f4dd7247c2bdd18d8cb3ee973f";
			var to = new EmailAddress(offer.UserEmail);
			var message = MailHelper.CreateSingleTemplateEmail(_address, to, templateId, new
			{
				userName = offer.UserEmail,
				prodYear = car.ProductionYear.ToString(),
				brand = car.Model.Brand.Name,
				model = car.Model.Name,
				price = offer.Price.ToString(),
				callbackUrl = $"https://localhost:7156/rentalconfirmation?offer_id={offer.OfferHashID}" // TODO
			});
			var response = await _client.SendEmailAsync(message);
		}
	}
	public class EmailServiceOptions
	{
		public string APIKey { get; set; } = string.Empty;
	}


}
