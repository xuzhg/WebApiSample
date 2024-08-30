namespace ODataDyncRouteSample.Models
{
    public class CommsEntity
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public ICollection<Availability> Availabilities { get; set; }

        public ICollection<string> Products { get; set; }
    }

    public class AzureUpdate : CommsEntity
    {
        public ICollection<string> ProductCategories { get; set; }

        public ICollection<string> UpdateTypes { get; set; }
    }

    public class M365Roadmap : CommsEntity
    {
        public ICollection<string> CloudInstances { get; set; }

        public ICollection<string> Platforms { get; set; }

        public ICollection<string> ReleaseRings { get; set; }

        public ICollection<string> MoreInfoUrls { get; set; }
    }

    public class Availability
    {
        public string Ring { get; set; }

        public int Year { get; set; }

        public string Month { get; set; }
    }
     
    public class ODataResponse<T>
    {
        public ODataValue<T> Value { get; set; }
    }

    public class ODataValue<T>
    {
        public ICollection<T> Content { get; set; }
    }
}
