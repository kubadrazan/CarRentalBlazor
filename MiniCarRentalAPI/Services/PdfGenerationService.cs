
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharedDataModels;

namespace MiniCarRentalAPI.Services
{
	public class PdfGenerationService(TimeProvider timeProvider)
	{
		public byte[] GenerateInvoice(Rental rental)
		{
			var localNow = timeProvider.GetLocalNow().DateTime;
			PdfDocument document = new PdfDocument();
			document.Info.Title = "Invoice " + localNow.ToString();

			PdfPage page = document.AddPage();
			XGraphics gfx = XGraphics.FromPdfPage(page);

			XFont font = new XFont("Arial", 12);
			XFont boldFont = new XFont("Arial", 14, XFontStyleEx.Bold);

			double marginLeft = 50;
			double marginTop = 50;
			double columnWidth = 100;

			double companyInfoX = marginLeft;
			double companyInfoY = marginTop;

			gfx.DrawString("MiNICarRental", boldFont, XBrushes.Black, new XPoint(companyInfoX, companyInfoY));
			companyInfoY += 20;
			gfx.DrawString("Address: Koszykowa 1, Warsaw", font, XBrushes.Black, new XPoint(companyInfoX, companyInfoY));
			companyInfoY += 20;
			gfx.DrawString("Email: minicarrental@hotmail.com", font, XBrushes.Black, new XPoint(companyInfoX, companyInfoY));

			double invoiceInfoY = companyInfoY + 40;
			gfx.DrawString("Invoice for:", boldFont, XBrushes.Black, new XPoint(companyInfoX, invoiceInfoY));
			invoiceInfoY += 20;
			gfx.DrawString($"Name: {rental.UserEmail}", font, XBrushes.Black, new XPoint(companyInfoX, invoiceInfoY));
			invoiceInfoY += 20;


			double tableX = (page.Width - 4 * columnWidth) / 2;
			double tableY = invoiceInfoY + 40;

			gfx.DrawString("ID", boldFont, XBrushes.Black, new XPoint(tableX, tableY));
			gfx.DrawString("Car", boldFont, XBrushes.Black, new XPoint(tableX + columnWidth, tableY));
			gfx.DrawString("Days", boldFont, XBrushes.Black, new XPoint(tableX + 2 * columnWidth, tableY));
			gfx.DrawString("Price per day", boldFont, XBrushes.Black, new XPoint(tableX + 3 * columnWidth, tableY));

			tableY += 20;
			gfx.DrawLine(XPens.Black, tableX, tableY, tableX + 4 * columnWidth, tableY);
			tableY += 20;

			gfx.DrawString(rental.ID.ToString(), font, XBrushes.Black, new XPoint(tableX, tableY));
			gfx.DrawString(rental.Car.ToString(), font, XBrushes.Black, new XPoint(tableX + columnWidth, tableY));
			gfx.DrawString((localNow - rental.RentDate.ToLocalTime()).Days.ToString(), font, XBrushes.Black, new XPoint(tableX + 2 * columnWidth, tableY));
			gfx.DrawString(rental.PricePerDay.ToString() + "$", font, XBrushes.Black, new XPoint(tableX + 3 * columnWidth, tableY));

			tableY += 20;
			tableY += 20;
			gfx.DrawString("Summary: ", boldFont, XBrushes.Black, new XPoint(tableX + 2 * columnWidth, tableY));
			gfx.DrawString((rental.PricePerDay * (localNow - rental.RentDate.ToLocalTime()).Days).ToString() + "$", font, XBrushes.Black, new XPoint(tableX + 3 * columnWidth, tableY));

			using (var memoryStream = new MemoryStream())
			{
				document.Save(memoryStream);
				return memoryStream.ToArray();
			}
		}

	}
}
