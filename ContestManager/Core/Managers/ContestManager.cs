using System;
using System.Linq;
using Core.DataBaseEntities;
using Core.Enums;
using Core.Factories;
using Core.Models;

namespace Core.Managers
{
    public interface IContestManager
    {
        Contest Create(string title, Guid ownerId, FieldDescription[] fields);
        void Update(Guid contestId, Guid? ownerId, ContestOptions options, FieldDescription[] fields);
        Contest Get(Guid contestId);
        Contest[] GetAll();
        News[] GetNews(Guid contestId);
        void AddNews(Guid contestId, string content);
    }

    public class ContestManager : IContestManager
    {
        private readonly IContextAdapterFactory contextFactory;

        public ContestManager(IContextAdapterFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public Contest Create(string title, Guid ownerId, FieldDescription[] fields)
        {
            var contest = new Contest
            {
                Id = Guid.NewGuid(),
                Title = title,
                OwnerId = ownerId,
                Fields = fields,
                CreationDate = DateTime.Now,
            };

            using (var db = contextFactory.Create())
            {
                db.AttachToInsert(contest);
                db.SaveChanges();
            }

            return contest;
        }

        public void Update(Guid contestId, Guid? ownerId, ContestOptions options, FieldDescription[] fields)
        {
            using (var db = contextFactory.Create())
            {
                var contest = db.ReadAndAttach<Contest>(contestId);

                if (ownerId.HasValue)
                    contest.OwnerId = ownerId.Value;
                contest.Options = options;
                contest.Fields = fields;

                db.SaveChanges();
            }
        }

        public Contest Get(Guid contestId)
        {
            using (var db = contextFactory.Create())
                return db.Read<Contest>(contestId);
        }

        public Contest[] GetAll()
        {
            using (var db = contextFactory.Create())
                return db.GetAll<Contest>();
        }

        public News[] GetNews(Guid contestId)
        {
            using (var db = contextFactory.Create())
                return db.GetAll<News>().Where(n => n.ContestId == contestId).ToArray();
        }

        public News AddNews(Guid contestId, string content)
        {
            var news = new News
            {
                ContestId = contestId,
                CreationDate = DateTime.Now,
                MdContent = content,
            };

            using (var db = contextFactory.Create())
            {
                db.AttachToInsert(news);
                db.SaveChanges();
            }

            return news;
        }
    }
}