namespace CollectionOfCollectionUsingEdmUntyped.Models
{
    public class PlaneDto
    {
        public Guid Id { get; set; }
        public double[] Normal { get; set; }
        public double[] Point { get; set; }
        public double[] Points { get; set; }
        public object Contours { get; set; }
    }
}
