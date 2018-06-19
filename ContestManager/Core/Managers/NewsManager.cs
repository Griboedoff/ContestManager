using System;
using System.Linq;
using Core.DataBaseEntities;
using Core.Factories;

namespace Core.Managers
{
    public interface INewsManager
    {
        News Create(string mdContent, Guid contestId);
        News Get(Guid newsId);
        News[] GetByContest(Guid contestId);
    }

    public class NewsManager : INewsManager
    {
        private readonly IContextAdapterFactory contextFactory;

        public NewsManager(IContextAdapterFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public News Create(string mdContent, Guid contestId)
        {
            var news = new News
            {
                Id = Guid.NewGuid(),
                ContestId = contestId,
                CreationDate = DateTime.Now,
                MdContent = mdContent,
            };

            using (var db = contextFactory.Create())
            {
                db.AttachToInsert(news);
                db.SaveChanges();
            }

            return news;
        }

        public News Get(Guid newsId)
        {
            using (var db = contextFactory.Create())
                return db.Read<News>(newsId);
        }

        public News[] GetByContest(Guid contestId)
        {
            using (var db = contextFactory.Create())
                return db.Set<News>()
                         .Where(n => n.ContestId == contestId)
                         .OrderBy(n => n.CreationDate)
                         .ToArray();
        }
    }
}    