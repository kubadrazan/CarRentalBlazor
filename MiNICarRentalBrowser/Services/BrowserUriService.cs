namespace MiNICarRentalBrowser.Services
{
    public class BrowserUriService
    {
        public BrowserUriService() { }

        public string AddCarBrandModelToUri(Uri uri, string? brand, string? model)
        {
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            query.Add("brand", brand);
            if (!String.IsNullOrEmpty(model))
                query.Add("model", model);

            return $"{uri.GetLeftPart(UriPartial.Path)}?{query}";
        }
    }
}
