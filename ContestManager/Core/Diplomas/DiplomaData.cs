using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataBaseEntities;

namespace Core.Diplomas
{
    public class DiplomaData
    {
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Class Class { get; set; }
        public string Coach { get; set; }
        public string Institution { get; set; }
        public string City { get; set; }

        public int? Position  { get; set; }
        public string Diplomas { get; set; }

        public string TemplatePath { get; set; }

        public bool HasPositionDiploma()
        {
            return Position.HasValue;
        }

        public Diploma GetPositionDiploma()
        {
            if (!HasPositionDiploma())
                return null;

            return CreateDiploma(
                Name,
                text: $"За {ToRoman(Position.Value)} место",
                type: GetTypeStr());
        }

        private string GetTypeStr()
        {
            return $"Учени{(Sex == Sex.Female ? "ца" : "к")} {Class:D} класса";
        }

        public bool HasCoachDiploma()
        {
            return Position.HasValue && Position.Value == 1 && !string.IsNullOrEmpty(Coach);
        }

        public IEnumerable<Diploma> GetCoachDiplomas()
        {
            if (!HasCoachDiploma())
                return null;

            return Coach
                .Split(new[] {';', ','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(coach => CreateDiploma(name: coach.Trim(), text: null, type: null))
                .ToList();
        }

        public bool HasOtherDiplomas()
        {
            return !string.IsNullOrEmpty(Diplomas);
        }

        public IEnumerable<Diploma> GetOtherDiplomas()
        {
            if (!HasOtherDiplomas())
                return null;

            var type = GetTypeStr();
            return Diplomas
                .Split(new[]{';', ','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(text => CreateDiploma(Name, text, type))
                .ToList();
        }

        public Diploma GetParticipationCertificate()
            => CreateDiploma(Name, text: null, type: GetTypeStr());

        private Diploma CreateDiploma(string name, string text, string type)
            => new Diploma(name, text, type, Institution, City, TemplatePath);

        private static readonly string[] Thou = { "", "M", "MM", "MMM" };
        private static readonly string[] Hun  = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
        private static readonly string[] Ten  = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        private static readonly string[] Ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

        private static string ToRoman(int num)
        {
            var roman = string.Empty;

            roman += Thou[num / 1000 % 10];
            roman += Hun[num / 100 % 10];
            roman += Ten[num / 10 % 10];
            roman += Ones[num % 10];

            return roman;
        }
    }
}
