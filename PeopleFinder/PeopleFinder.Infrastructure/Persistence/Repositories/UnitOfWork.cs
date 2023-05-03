using System.Data;
using PeopleFinder.Domain.Repositories;
using PeopleFinder.Domain.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;



namespace PeopleFinder.Infrastructure.Persistence.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {

        private readonly PeopleFinderDbContext _dbContext;
        private IProfileRepository? _profileRepository;
        private IUserRepository? _userRepository;
        private ITagRepository? _tagRepository;
        private IRecommendationRepository? _recommendationRepository;
        private IChatRepository? _chatRepository;
        private IRelationshipRepository? _relationshipRepository;
        private IMediaFileRepository? _mediaFileRepository;
        public UnitOfWork(PeopleFinderDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        
        public IMediaFileRepository MediaFileRepository => _mediaFileRepository ??= new MediaFileRepository(_dbContext);
        public IProfileRepository ProfileRepository => _profileRepository ??= new ProfileRepository(_dbContext);

        public ITagRepository TagRepository => _tagRepository ??= new TagRepository(_dbContext);

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_dbContext);
        
        public IRecommendationRepository RecommendationRepository => _recommendationRepository ??= new RecommendationRepository(_dbContext);
        public IChatRepository ChatRepository => _chatRepository ??= new ChatRepository(_dbContext);
        public IRelationshipRepository RelationshipRepository => _relationshipRepository ??= new RelationshipRepository(_dbContext);

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return _dbContext.Database.BeginTransaction(isolationLevel);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return _dbContext.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       
    }
}
