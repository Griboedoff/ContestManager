using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataBaseEntities;
using Core.Helpers;

namespace Core.Contests
{
    public interface ISeatingGenerator
    {
        IReadOnlyList<Participant> Generate(
            IReadOnlyList<Participant> participants,
            IEnumerable<Auditorium> auditoriums);
    }

    public class SeatingGenerator : ISeatingGenerator
    {
        private readonly IDataGenerator dataGenerator;
        private const int TwoClassesUpperBound = 80;
        private const int FourClassesLowerBound = 150;
        private readonly HashSet<string> generatedData = new HashSet<string>();

        public SeatingGenerator(IDataGenerator dataGenerator) => this.dataGenerator = dataGenerator;

        public IReadOnlyList<Participant> Generate(
            IReadOnlyList<Participant> participants,
            IEnumerable<Auditorium> auditoriums)
        {
            var sortedParticipants = new Dictionary<Class, (Stack<Participant> used, Stack<Participant> free)>();
            foreach (var participant in participants)
            {
                if (!participant.UserSnapshot.Class.HasValue)
                    throw new ArgumentException($"User with null class {participant.UserId}");

                var partClass = participant.UserSnapshot.Class.Value;
                if (!sortedParticipants.ContainsKey(partClass))
                    sortedParticipants[partClass] = (new Stack<Participant>(), new Stack<Participant>());

                sortedParticipants[partClass].free.Push(participant);
            }

            foreach (var auditorium in auditoriums)
                FillAuditorium(auditorium, sortedParticipants);

            return participants;
        }

        private void FillAuditorium(
            Auditorium auditorium,
            Dictionary<Class, (Stack<Participant> used, Stack<Participant> free)> sortedParticipants)
        {
            var partsCount = GetPartsCount(auditorium.Capacity);
            var left = auditorium.Capacity;
            var partSize = auditorium.Capacity / partsCount;
            var usedClasses = new HashSet<Class>();
            for (var i = 0; i < partsCount; i++)
            {
                var partCountNeeded = Math.Min(left, partSize);
                var (@class, (used, free)) = sortedParticipants
                    .OrderByDescending(v => v.Value.free.Count)
                    .First(v => !usedClasses.Contains(v.Key));
                usedClasses.Add(@class);

                for (var j = 0; j < partCountNeeded; j++)
                {
                    if (free.Any())
                        break;

                    var participant = free.Pop();
                    var (login, pass) = GenerateLogin(participant, auditorium.Code);
                    participant.Login = login;
                    participant.Pass = pass;
                    used.Push(participant);
                }
            }
        }

        private (string, string) GenerateLogin(Participant participant, string auditoriumCode)
        {
            var @class = participant.UserSnapshot.Class.Value;
            var source = $"{DataGenerator.Numbers}{DataGenerator.EnglishUppercase}";

            return (
                GenerateUnique(() => $"{@class:D}-{auditoriumCode}-{dataGenerator.GenerateSequence(5, source)}"),
                GenerateUnique(() => $"{ClassLetterMap[@class]}{dataGenerator.GenerateSequence(5, source)}"));
        }

        private string GenerateUnique(Func<string> generator)
        {
            var value = generator();
            while (generatedData.Contains(value))
                value = generator();

            generatedData.Add(value);
            return value;
        }

        private static int GetPartsCount(int capacity) => capacity < TwoClassesUpperBound
            ? 2
            : capacity > FourClassesLowerBound
                ? 4
                : 3;

        private static readonly Dictionary<Class, char> ClassLetterMap = new Dictionary<Class, char>
        {
            [Class.Eleventh] = 'A',
            [Class.Tenth] = 'B',
            [Class.Ninth] = 'C',
            [Class.Eighth] = 'D',
            [Class.Seventh] = 'E',
            [Class.Sixth] = 'F',
            [Class.Fifth] = 'G',
        };
    }
}