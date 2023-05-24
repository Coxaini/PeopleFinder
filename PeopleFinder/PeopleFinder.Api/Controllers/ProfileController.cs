using Microsoft.AspNetCore.Mvc;
using PeopleFinder.Application.Models.Profile;
using PeopleFinder.Application.Services.ProfileServices;
using Microsoft.AspNetCore.Http.HttpResults;
using PeopleFinder.Api.Common.Extensions;
using System.Linq;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using MapsterMapper;
using PeopleFinder.Contracts.Profile;
using Microsoft.AspNetCore.Authorization;
using PeopleFinder.Api.Controllers.Common;
using System.Security.Claims;
using Newtonsoft.Json;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Services.FileStorage;
using PeopleFinder.Application.Services.RelationshipServices;
using PeopleFinder.Contracts.Pagination;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Api.Controllers
{
    [Route("/profile")]
    
    public class ProfileController : ApiController
    {
        private readonly IProfileService _profileService;
        private readonly IRelationshipService _relationshipService;
        private readonly IFileTypeResolver _fileTypeResolver;
        private readonly IMapper _mapper;
        public ProfileController(IProfileService profileService,  IMapper mapper, IRelationshipService relationshipService, IFileTypeResolver fileTypeResolver)
        {
            _profileService = profileService;
            _mapper = mapper;
            _relationshipService = relationshipService;
            _fileTypeResolver = fileTypeResolver;
        }


        [HttpPost]      
        public async Task<IActionResult>CreateProfile(ProfileFillRequest request)
        {

            var createResult = await _profileService.CreateProfile(UserIdInClaims, request);

            return createResult.Match(
                (profile) => {
                    return Created(Request.GetUri() + $"/{profile.Id}", _mapper.Map<ShortProfileResponse>(profile));
                    },
                Problem);
        }
        [HttpPut]
        public async Task<IActionResult> EditProfile(ProfileFillRequest request)
        {
            var editResult = await _profileService.UpdateProfile(ProfileIdInClaims, request);

            return editResult.Match(
                (profile) => Ok(_mapper.Map<ShortProfileResponse>(profile)),
                Problem);
        }
        
        [HttpPost("picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile imageFile)
        {
            
            FileDto fileDto = new(imageFile.FileName, imageFile.OpenReadStream(), MediaFileType.Image);
            
            var result = await _profileService.UploadProfilePicture(ProfileIdInClaims, fileDto);

            
            return result.Match(
                (image) => CreatedAtAction(nameof(FileController.GetFile), "File", 
                    new{ Token = image.Id} , image),
                Problem);
        }
        
        [HttpGet("{profileId:int}")]
        public async Task<IActionResult> GetProfileById(int profileId)
        {
            var profile = await _profileService.GetProfileWithRelationshipById(profileId, ProfileIdInClaims);

            return profile.Match(
                     (source) =>
                     {
                         var metadata = new
                         {
                             source.MutualFriends.TotalCount,
                             NextCursor =  source.MutualFriends.Next?.Relationship?.AcknowledgeAt,
                         };
                         Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                         
                         return Ok(_mapper.Map<ProfileResponse>(source));
                     },
                     Problem);

        }
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfileByUsername(string username)
        {
            var profile = await _profileService.GetProfileWithRelationshipByUsername(username, ProfileIdInClaims);
            return profile.Match(
                (source) =>
                {
                    var metadata = new
                    {
                        source.MutualFriends.TotalCount,
                        NextCursor =  source.MutualFriends.Next?.Relationship?.AcknowledgeAt,
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                         
                    var res = _mapper.Map<ProfileResponse>(source);
                    return Ok(res);
                },
                Problem);
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> SearchProfile([FromQuery]string searchQuery, [FromQuery]CursorPagination<DateTime> cursorPaginationParams)
        {
            CursorPaginationParams<DateTime> pag = new(20)
                { PageSize = cursorPaginationParams.PageSize, After = cursorPaginationParams.After, Before = cursorPaginationParams.Before };
            
            var profiles = await _profileService.GetProfilesByFilter(ProfileIdInClaims, pag, searchQuery);
            return profiles.Match(
                (source) =>
                {
                    var metadata = new
                    {
                        source.TotalCount,
                        NextCursor =  source.Next?.LastActivity,
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                         
                    return Ok(_mapper.Map<IList<ShortProfileResponse>>(source.Items));
                },
                Problem);
        }

        [HttpGet("{profileId:int}/mutualFriends")]
        public async Task<IActionResult> GetMutualFriendsWithProfile(int profileId, [FromQuery]CursorPagination<DateTime> cursorPaginationParams)
        {
            CursorPaginationParams<DateTime> pag = new(20)
                { PageSize = cursorPaginationParams.PageSize, After = cursorPaginationParams.After, Before = cursorPaginationParams.Before };
            
            var mutualFriends = await _relationshipService.GetMutualFriends(ProfileIdInClaims, profileId, pag);
            return mutualFriends.Match(
                (source) =>
                {
                    var metadata = new
                    {
                        source.TotalCount,
                        NextCursor =  source.Next?.Relationship?.AcknowledgeAt,
                    };
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                         
                    return Ok(_mapper.Map<IList<ShortProfileResponse>>(source.Items));
                },
                Problem);
            
        }
        
    }
}
