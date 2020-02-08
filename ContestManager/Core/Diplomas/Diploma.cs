using System.Text.RegularExpressions;

namespace Core.Diplomas
{
    public class Diploma
    {
        private static readonly Regex Regex = new Regex(@"^(город\s|г\.\s|г\.)");

        public readonly string Name;
        public readonly string Text;
        public readonly string Type;
        public readonly string Institution;
        public readonly string City;

        public readonly string TemplatePath;

        public Diploma(string name, string text, string type, string institution, string city, string templatePath)
        {
            Name = name;
            Text = text;
            Type = type;
            Institution = institution;
            TemplatePath = templatePath;

            if (!string.IsNullOrEmpty(city))
                City = $"г. {Regex.Replace(city, string.Empty)}";
        }

        private bool Equals(Diploma other)
        {
            return string.Equals(Name, other.Name) && string.Equals(City, other.City);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Diploma) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (City != null ? City.GetHashCode() : 0);
            }
        }
    }
}
