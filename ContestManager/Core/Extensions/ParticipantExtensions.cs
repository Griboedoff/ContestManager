using System.Globalization;
using System.Linq;
using Core.DataBaseEntities;

namespace Core.Extensions
{
    public static class ParticipantExtensions
    {
        public static double[] ResultsAsNumbers(this Participant participant)
            => participant.Results.Select(i => double.Parse(i, CultureInfo.GetCultureInfo("RU-ru"))).ToArray();
    }
}
