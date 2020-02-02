using System;
using System.Threading.Tasks;
using Core.DataBase;
using Core.DataBaseEntities;
using Microsoft.Extensions.Caching.Memory;

namespace Core.Users.Sessions
{
    public interface ISessionManager
    {
        Task<Guid> CreateSession(User user);
        Task<bool> ValidateSession(Guid sid, Guid userId);
    }

    public class SessionManager : ISessionManager
    {
        private static readonly TimeSpan SessionTtl = TimeSpan.FromDays(1);
        private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(1);

        private readonly IAsyncRepository<Session> sessionRepo;
        private readonly IMemoryCache memoryCache;

        public SessionManager(IAsyncRepository<Session> sessionRepo, IMemoryCache memoryCache)
        {
            this.sessionRepo = sessionRepo;
            this.memoryCache = memoryCache;
        }

        public async Task<Guid> CreateSession(User user)
        {
            var sid = Guid.NewGuid();

            var session = new Session
            {
                Id = sid,
                UserId = user.Id,
                LastUse = DateTimeOffset.UtcNow,
            };
            await sessionRepo.AddAsync(session);
            memoryCache.Set(sid, session, CacheTtl);

            return sid;
        }

        public async Task<bool> ValidateSession(Guid sid, Guid userId)
        {
            if (memoryCache.TryGetValue<Session>(sid, out var session))
                return session.UserId == userId;

            session = await sessionRepo.FirstOrDefaultAsync(s => s.Id == sid);
            if (session == null ||
                session.LastUse.Add(SessionTtl).ToUniversalTime() < DateTimeOffset.UtcNow ||
                session.UserId != userId)
                return false;

            session.LastUse = DateTimeOffset.UtcNow;
            await sessionRepo.UpdateAsync(session);
            memoryCache.Set(sid, session, CacheTtl);
            return true;
        }
    }
}
