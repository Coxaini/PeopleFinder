using PeopleFinder.Contracts.Profile;

namespace PeopleFinder.Contracts.Rating;


public record FriendRequestResponse(long RequestId , ShortProfileResponse Profile , DateTime SentAt);