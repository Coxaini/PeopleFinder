using FluentResults;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Application.Services.Tags;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;

    public TagService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<List<UserTag>>> GetTags()
    {
        return await _unitOfWork.TagRepository.GetAll();

    }
}