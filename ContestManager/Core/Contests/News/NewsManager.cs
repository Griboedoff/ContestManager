﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBase;
using NewsModel = Core.DataBaseEntities.News;

namespace Core.Contests.News
{
    public interface INewsManager
    {
        Task<NewsModel> Create(CreateNewsModel createNewsModel);
        Task<NewsModel> Update(Guid id, CreateNewsModel createNewsModel);
        Task<bool> Exists(Guid id);
        Task<NewsModel> Get(Guid newsId);
        Task<NewsModel[]> GetByContest(Guid contestId);
    }

    public class NewsManager : INewsManager
    {
        private readonly IAsyncRepository<NewsModel> newsRepo;

        public NewsManager(IAsyncRepository<NewsModel> newsRepo)
        {
            this.newsRepo = newsRepo;
        }

        public async Task<NewsModel> Create(CreateNewsModel createNewsModel)
        {
            var news = new NewsModel
            {
                Id = Guid.NewGuid(),
                ContestId = createNewsModel.ContestId,
                CreationDate = DateTime.Now,
                Content = createNewsModel.Content,
                Title = createNewsModel.Title,
            };

            return await newsRepo.AddAsync(news);
        }

        public async Task<NewsModel> Update(Guid id, CreateNewsModel createNewsModel)
        {
            var news = await newsRepo.GetByIdAsync(id);

            news.CreationDate = DateTime.Now;
            news.Content = createNewsModel.Content;
            news.Title = createNewsModel.Title;

            return await newsRepo.UpdateAsync(news);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await newsRepo.GetByIdAsync(id) != null;
        }

        public async Task<NewsModel> Get(Guid newsId) => await newsRepo.GetByIdAsync(newsId);

        public async Task<NewsModel[]> GetByContest(Guid contestId)
            => (await newsRepo.WhereAsync(n => n.ContestId == contestId)).OrderBy(n => n.CreationDate).ToArray();
    }
}
