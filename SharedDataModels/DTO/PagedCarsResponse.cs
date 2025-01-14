namespace SharedDataModels.DTO
{
    public class PagedCarsResponse
    {
        public List<SimpleCarDTO> Cars { get; set; }
        public int TotalCount { get; set; }
    }
}
