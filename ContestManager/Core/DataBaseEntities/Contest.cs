using System;
using Core.Enums;

namespace Core.DataBaseEntities
{
    public class Contest : DataBaseEntity
    {
        public string Title { get; set; }

        public Guid OwnerId { get; set; }

        public ContestType Type { get; set; }

        public ContestState State { get; set; }

        public DateTime CreationDate { get; set; }
    }

    public enum ContestState
    {
        RegistrationOpen = 0,
        RegistrationClosed = 100,
        Running = 200,
        Checking = 300,
        Finished = 400,
    }
}