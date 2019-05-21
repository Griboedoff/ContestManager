using Core.Enums;

namespace Core.Contests
{
    public class CreateContestModel
    {
        public string Title { get; set; }
        public ContestType Type { get; set; }
    }
}