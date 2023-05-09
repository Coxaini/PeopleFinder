using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace PeopleFinder.Domain.Repositories.Common
{
    public interface IUnitOfWork : IDisposable
    {

        IProfileRepository ProfileRepository { get; }
        ITagRepository TagRepository { get; }
        IUserRepository UserRepository { get; }
        IRecommendationRepository RecommendationRepository { get; }
        IChatRepository ChatRepository { get; }
        IMediaFileRepository MediaFileRepository { get; }
        
        IMessageRepository MessageRepository { get; }

        IRelationshipRepository RelationshipRepository { get; }
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        
        Task<int> SaveAsync();
    }
}
