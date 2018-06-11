using System;
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
    }
}