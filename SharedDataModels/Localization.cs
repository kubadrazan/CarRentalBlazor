namespace SharedDataModels
{
	public class Localization
    {
        public int ID { get; set; }

        // TODO specify localization properties, possibly create more tables like Countries, Cities...

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int HouseNumber { get; set; }

        public ICollection<Car> Cars { get; set; }
        public ICollection<Return> Returns { get; set; }
    }
}
