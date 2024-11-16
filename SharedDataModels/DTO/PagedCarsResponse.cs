namespace SharedDataModels.DTO
{
    public class PagedCarsResponse
    {
        public List<Car> Cars { get; set; }
        public int TotalCount { get; set; }
    }
}
