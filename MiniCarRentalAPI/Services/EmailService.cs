using Humanizer;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MiniCarRentalAPI.Controllers;
using SendGrid;
using SendGrid.Helpers.Mail;
using SharedDataModels;
using System;
using System.Drawing.Drawing2D;

namespace MiniCarRentalAPI.Services
{
	public class EmailService
	{
		private readonly SendGridClient _client;
		private readonly PdfGenerationService _pdfGenerationService;
		private readonly EmailAddress _address;
		private readonly SendGridMessage _message;
		private readonly IConfiguration _configuration;
		private readonly TimeProvider _timeProvider;
		private readonly string _apiA;

		public EmailService(IConfiguration configuration, IOptions<EmailServiceOptions> options, PdfGenerationService pdfGenerationService, TimeProvider timeProvider)
		{

			_client = new SendGridClient(options.Value.APIKey);
			_address = new EmailAddress("minicarrental@hotmail.com");
			_pdfGenerationService = pdfGenerationService;
			_configuration = configuration;
			_timeProvider = timeProvider;
#if DEBUG
			_apiA = configuration.GetValue<string>("ApiUrls:ApiBrowserA") ?? throw new Exception("No apiA Url in configuration file!");
#else
			_apiA = configuration.GetValue<string>("aApiUrl") ?? throw new Exception("No apiA Url in Azure key vault!");
#endif
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
				callbackUrl = $"{_apiA}/rentalconfirmation?offer_id={offer.OfferGuid}" // TODO
			});
			var response = await _client.SendEmailAsync(message);
		}
		public async void SendInvoice(Rental rental)
		{
			string templateId = "d-1875804a5f7349978f3f1daadee834ef";
			var to = new EmailAddress(rental.UserEmail);
			var localNow = _timeProvider.GetLocalNow().DateTime;
			var message = MailHelper.CreateSingleTemplateEmail(_address, to, templateId, new
			{
				userName = rental.UserEmail,
				prodYear = rental.Car.ProductionYear.ToString(),
				brand = rental.Car.Model.Brand.Name,
				model = rental.Car.Model.Name,
				InvoiceNumber = "IN-" + localNow.ToString("yyyy-MM-dd-hhmmss"),
				IssueDate = localNow.ToString(),
				DueDate = localNow.AddDays(14).ToString(),
				Amount = (rental.PricePerDay * (localNow - rental.RentDate.ToLocalTime()).Days).ToString() + "$"

			});
			message.AddAttachment("Invoice.pdf", Convert.ToBase64String(_pdfGenerationService.GenerateInvoice(rental)));

			var response = await _client.SendEmailAsync(message);
		}
	}

	public class EmailServiceOptions
	{
		public string APIKey { get; set; } = string.Empty;
	}
}
