using System;

namespace Core.Contests.News
{
    public class CreateNewsModel
    {
        public Guid ContestId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}