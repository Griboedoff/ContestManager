using System;

namespace Core.Contests
{
    public class Result
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SchoolWithCity { get; set; }
        public int[] Results { get; set; }
        public int Sum { get; set; }
        public string Place { get; set; }
    }
}
