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
        public Distance()
        {

        }

        public Distance(string value)
        {
            int index = 0;
            for (; index < value.Length; index++)
            {
                if (!char.IsDigit(value[index]))
                {
                    break;
                }
            }

            string left = value.Substring(0, index);
            string right = value.Substring(index);

            Value = double.Parse(left);
            Unit = Enum.Parse<DistanceUnit>(right.ToUpper());
        }

        public double Value { get; set; }

        public DistanceUnit Unit { get; set; }

        public override string ToString()
        {
            return $"{Value}{Unit.ToString().ToLowerInvariant()}";
        }
    }
}
