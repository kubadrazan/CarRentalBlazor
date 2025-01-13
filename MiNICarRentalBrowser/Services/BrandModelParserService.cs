using SharedDataModels.DTO;

namespace MiNICarRentalBrowser.Services
{
    public class BrandModelParserService
    {
        public void ParseBrandsModels(List<BrandModelDTO> brandsModels, List<string> brands, Dictionary<string, List<string>> models)
        {
            foreach (var m in brandsModels)
            {
                if (models.ContainsKey(m.BrandName))
                    models[m.BrandName].Add(m.ModelName);
                else
                {
                    brands.Add(m.BrandName);
                    models[m.BrandName] = new() { m.ModelName };
                }
            }
        }
    }
}
