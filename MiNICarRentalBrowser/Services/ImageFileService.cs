using Microsoft.AspNetCore.Components.Forms;

namespace MiNICarRentalBrowser.Services
{
    public class ImageFileService
    {

        public async Task<byte[]> LoadBrowserFileTOBuffAsync(IBrowserFile file)
        {
            var resizedFile = await file.RequestImageFileAsync(file.ContentType, 300, 300);
            var buf = new byte[resizedFile.Size];
            using (var stream = resizedFile.OpenReadStream())
            {
                await stream.ReadAsync(buf);
            }
            return buf;
        }
    }
}
