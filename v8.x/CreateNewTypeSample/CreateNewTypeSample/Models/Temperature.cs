namespace CreateNewTypeSample.Models
{
    public enum TemperatureKind
    {
        Celsius,

        Fahrenheit
    }

    public class Temperature
    {
        public Temperature(string temp)
        {
            if (temp.EndsWith("℃"))
            {
                Kind = TemperatureKind.Celsius;

                double.TryParse(temp.Substring(0, temp.Length - 1), out double value);
                Celsius = value;
                Fahrenheit = ToFahrenheit(Celsius);
            }
            else if (temp.EndsWith("℉"))
            {
                Kind = TemperatureKind.Fahrenheit;

                double.TryParse(temp.Substring(0, temp.Length - 1), out double value);
                Fahrenheit = value;
                Celsius = ToCelsius(Fahrenheit);
            }
            else
            {
                throw new Exception("Unknow format for temperature");
            }


        }

        public Temperature(double temp, TemperatureKind kind)
        {
            Kind = kind;

            if (kind == TemperatureKind.Celsius)
            {
                Celsius = temp;
                Fahrenheit = ToFahrenheit(temp);
            }
            else
            {
                Celsius = ToCelsius(temp);
                Fahrenheit = temp;
            }
        }

        public TemperatureKind Kind { get; }

        public double Celsius { get; }

        public double Fahrenheit { get; }

        private static double ToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        private static double ToFahrenheit(double celsius)
        {
            return celsius * 9 / 5 + 32;
        }

        public override string ToString()
        {
            if (Kind == TemperatureKind.Celsius)
            {
                return $"{Celsius:0.00}\x2103";
            }
            else
            {
                return $"{Fahrenheit:0.00}\x2109";
            }
        }
    }
}
