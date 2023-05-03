using FluentResults;
using PeopleFinder.Domain.Entities.MessagingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Services.ChatServices
{
    public interface IChatService
    {
        Task<Result<Chat>> CreateChat();
        Task<Result<Chat>> UpdateChat();
        Task<Result<Chat>> DeleteChat();
    }
}
