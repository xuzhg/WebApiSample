namespace GenericControllerSample.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Annotation Annotation { get; set; }

        public Attachement Attach { get; set; }
    }

    public class Annotation
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class Attachement
    {
        public int Id { get; set; }
    }
}
