using PeopleFinder.Application.Models.File;

namespace PeopleFinder.Application.Models.Chat;

public record CreateDirectChatRequest(int CreatorId,int FriendId);