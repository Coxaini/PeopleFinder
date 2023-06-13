using FluentResults;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Application.Services.Tags;

public interface ITagService
{
    Task<Result<List<UserTag>>> GetTags();
}