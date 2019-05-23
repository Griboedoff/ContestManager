using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;

namespace Core.News_
{
    public interface INewsManager
    {
        Task<News> Create(CreateNewsModel createNewsModel);
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

        public async Task<News> Create(CreateNewsModel createNewsModel)
        {
            var news = new News
            {
                Id = Guid.NewGuid(),
                ContestId = createNewsModel.ContestId,
                CreationDate = DateTime.Now,
                Content = createNewsModel.Content,
                Title = createNewsModel.Title,
            };

            return await newsRepo.AddAsync(news);
        }

        public async Task<News> Get(Guid newsId) => await newsRepo.GetByIdAsync(newsId);

        public async Task<News[]> GetByContest(Guid contestId)
            => (await newsRepo.WhereAsync(n => n.ContestId == contestId)).OrderBy(n => n.CreationDate).ToArray();
    }
}