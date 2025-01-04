
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SharedDataModels;

namespace MiniCarRentalAPI.Services
{
	public class PdfGenerationService
	{
		string title = "Invoice";
		public byte[] GenerateInvoice(Rental rental)
		{
			PdfDocument document = new PdfDocument();
			document.Info.Title = title+DateTime.Now.ToString();

			PdfPage page = document.AddPage();
			XGraphics gfx = XGraphics.FromPdfPage(page);

			XFont font = new XFont("Arial", 12);

			double x = 50;
			double y = 50;
			double columnWidth = 100;

			gfx.DrawString("ID", font, XBrushes.Black, new XPoint(x, y));
			gfx.DrawString("Car", font, XBrushes.Black, new XPoint(x + columnWidth, y));
			gfx.DrawString("Price", font, XBrushes.Black, new XPoint(x + 2 * columnWidth, y));

			y += 20;
			gfx.DrawLine(XPens.Black, x, y, x + 3 * columnWidth, y);
			y += 20;
			gfx.DrawString(rental.ID.ToString(), font, XBrushes.Black, new XPoint(x, y));
				gfx.DrawString("TEMP", font, XBrushes.Black, new XPoint(x + columnWidth, y));
				gfx.DrawString((rental.PricePerDay*(DateTime.Now-rental.RentDate).Days).ToString(), font, XBrushes.Black, new XPoint(x + 2 * columnWidth, y));

				y += 20;
			using (var memoryStream = new MemoryStream())
			{
				document.Save(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}
