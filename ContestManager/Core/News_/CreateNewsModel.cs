using System;

namespace Core.News_
{
    public class CreateNewsModel
    {
        public Guid ContestId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}