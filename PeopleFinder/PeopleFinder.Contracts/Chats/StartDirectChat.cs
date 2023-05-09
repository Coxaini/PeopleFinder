using Microsoft.AspNetCore.Http;

namespace PeopleFinder.Contracts.Chats;

public record StartDirectChat(int FriendId, string Text, IFormFile? Attachment );