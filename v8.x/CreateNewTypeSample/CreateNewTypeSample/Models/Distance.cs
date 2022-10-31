namespace CreateNewTypeSample.Models
{
    public enum DistanceUnit
    {
        M,
        CM,
        FT,
        IN,
    }

    public class Distance
    {
        public double Value { get; set; }

        public DistanceUnit Unit { get; set; }

        public override string ToString()
        {
            return $"{Value}{Unit.ToString().ToLowerInvariant()}";
        }
    }
}
