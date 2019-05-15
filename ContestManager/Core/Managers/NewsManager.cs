using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers
{
    public interface INewsManager
    {
        Task<News> Create(string mdContent, Guid contestId);
        Task<News> Get(Guid newsId);
        Task<News[]> GetByContest(Guid contestId);
    }

    public class NewsManager : INewsManager
    {
        public async Task<News> Create(string mdContent, Guid contestId)
        {
            using var db = new Context();

            var news = new News
            {
                Id = Guid.NewGuid(),
                ContestId = contestId,
                CreationDate = DateTime.Now,
                MdContent = mdContent,
            };

            db.News.Add(news);
            await db.SaveChangesAsync();

            return news;
        }

        public async Task<News> Get(Guid newsId)
        {
            using var db = new Context();

            return await db.News.Read(newsId);
        }

        public async Task<News[]> GetByContest(Guid contestId)
        {
            using var db = new Context();

            return await db.Set<News>()
                .Where(n => n.ContestId == contestId)
                .OrderBy(n => n.CreationDate)
                .ToArrayAsync();
        }
    }
}