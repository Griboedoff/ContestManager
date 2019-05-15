using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;

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
        private readonly IAsyncRepository<News> newsRepo;

        public NewsManager(IAsyncRepository<News> newsRepo)
        {
            this.newsRepo = newsRepo;
        }

        public async Task<News> Create(string mdContent, Guid contestId)
        {
            var news = new News
            {
                Id = Guid.NewGuid(),
                ContestId = contestId,
                CreationDate = DateTime.Now,
                MdContent = mdContent,
            };

            return await newsRepo.AddAsync(news);
        }

        public async Task<News> Get(Guid newsId) => await newsRepo.GetByIdAsync(newsId);

        public async Task<News[]> GetByContest(Guid contestId)
            => (await newsRepo.WhereAsync(n => n.ContestId == contestId)).OrderBy(n => n.CreationDate).ToArray();
    }
}