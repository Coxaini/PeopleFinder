using System.Text;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeopleFinder.Api.Common.Extensions;
using PeopleFinder.Api.Controllers.Common;
using PeopleFinder.Application.Models.Friend;
using PeopleFinder.Application.Models.Rating;
using PeopleFinder.Application.Services.RelationshipServices;
using PeopleFinder.Contracts.Pagination;
using PeopleFinder.Contracts.Profile;
using PeopleFinder.Contracts.Rating;
using PeopleFinder.Contracts.Recommendation;
using PeopleFinder.Domain.Common.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Common.Pagination.Page;

namespace PeopleFinder.Api.Controllers
{
    [Route("/friends")]
    public class FriendController : ApiController
    {
        private readonly IRelationshipService _relationshipService;
        private  readonly  IMapper _mapper;

        public FriendController(IRelationshipService relationshipService, IMapper mapper)
        {
            _relationshipService = relationshipService;
            _mapper = mapper;
        }

        [HttpPost("request")]
        public async Task<IActionResult> SendFriendRequestToProfile(FriendshipRequest request)
        {
            var requestResult = await _relationshipService.SendFriendRequest(ProfileIdInClaims, request);
            
            return requestResult.Match(
                (friendRequest) => Ok("Friend request sent successfully"),
                Problem);
        }
        
        
        
        [HttpDelete("{friendId:int}")]
        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            var requestResult = await _relationshipService.RemoveFriend(ProfileIdInClaims, friendId);
            
            return requestResult.Match(
                () => Ok("Friend removed successfully"),
                Problem);
        }
        
        [HttpPost("requests/{friendRequesterId:int}")]
        public async Task<IActionResult> ApproveFriendRequest(int friendRequesterId)
        {
            var requestResult = await _relationshipService.ApproveFriendRequest(ProfileIdInClaims, friendRequesterId);
            
            return requestResult.Match(
                () => Ok("Friend request approved successfully"),
                Problem);
        }
        
        [HttpPut("requests/{friendRequesterId:int}")]
        public async Task<IActionResult> RejectFriendRequest(int friendRequesterId)
        {
            var requestResult = await _relationshipService.RejectFriendRequest(ProfileIdInClaims, friendRequesterId);
            
            return requestResult.Match(
                () => Ok("Friend request rejected successfully"),
                Problem);
        }
        
        [HttpDelete("requests/{receiverProfileId:int}")]
        public async Task<IActionResult> CancelFriendRequest(int receiverProfileId)
        {
            var requestResult = await _relationshipService.CancelFriendRequest(ProfileIdInClaims, receiverProfileId);
            
            return requestResult.Match(
                () => Ok("Friend request cancelled successfully"),
                Problem);
        }
        
        
        [HttpGet("")]
        public async Task<IActionResult> GetFriends([FromQuery]CursorPagination<DateTime> paginationParams, 
            [FromQuery]string? searchQuery)
        {
            CursorPaginationParams<DateTime> pag = new(20)
                { PageSize = paginationParams.PageSize, After = paginationParams.After, Before = paginationParams.Before };

            var friendsResult = await _relationshipService.GetFriends(ProfileIdInClaims, pag, searchQuery);
            return friendsResult.Match(
                friends =>
                {
                    var metadata = new
                    {
                        friends.TotalCount,
                        NextCursor =  friends.Next?.Relationship?.AcknowledgeAt,
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    
                    return Ok(_mapper.Map<IList<ShortProfileResponse>>(friends.Items));
                },
                Problem);
           
        }
        
        [HttpGet("updates")]
        public async Task<IActionResult> GetFriendRequestUpdates([FromQuery]CursorPagination<DateTime> paginationParams)
        {
            CursorPaginationParams<DateTime> pag = new(20)
                { PageSize = paginationParams.PageSize, After = paginationParams.After, Before = paginationParams.Before };

            var updatesResult = await _relationshipService.GetFriendshipRequestUpdates(ProfileIdInClaims, pag);

            return updatesResult.Match(
                updates =>
                {
                    var metadata = new
                    {
                        updates.TotalCount,
                        NextCursor =  updates.Next?.SentAt,
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    
                    return Ok(_mapper.Map<IList<FriendRequestResponse>>(updates.Items));
                },
                Problem);
            
        }

        /*[HttpGet("updates")]
        public async Task<IActionResult> GetFriendRequestUpdates([FromQuery]PaginationRequestParams paginationParams)
        {
            PagedPaginationParams pag = new()
                { PageNumber = paginationParams.PageNumber, PageSize = paginationParams.PageSize };

            var updatesResult = await _friendsService.GetFriendshipRequestUpdates(ProfileIdInClaims, pag);

            return updatesResult.Match(
                updates =>
                {
                    var metadata = new
                    {
                        updates.TotalCount,
                        updates.PageSize,
                        updates.CurrentPage,
                        updates.TotalPages,
                        updates.HasNext,
                        updates.HasPrevious
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    
                    return Ok(_mapper.Map<IEnumerable<FriendRequestResponse>>(updates));
                },
                Problem);
            
        }*/
        

    }
}
