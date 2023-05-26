using FluentResults;
using FluentValidation;
using PeopleFinder.Application.Common.Errors;
using PeopleFinder.Application.Common.Extensions;
using PeopleFinder.Application.Models.Profile;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Repositories.Common;
using MapsterMapper;
using PeopleFinder.Application.Common.Interfaces.FileStorage;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Domain.Common.Models;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageManager _storageManager;
        private readonly IValidator<ProfileFillRequest> _profileFillValidator;
        private readonly IValidator<FileDto> _profileImageValidator;
        private readonly IMapper _mapper;

        public ProfileService(IUnitOfWork unitOfWork, IFileStorageManager storageManager, IValidator<ProfileFillRequest> profileFillValidator
            ,IValidator<FileDto> profileImageValidator , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _storageManager = storageManager;
            _profileFillValidator = profileFillValidator;
            _profileImageValidator = profileImageValidator;
            _mapper = mapper;
        }

        public async Task<Result<Profile>> CreateProfile(int userId ,ProfileFillRequest request)
        {


            var result = await _profileFillValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return Result.Fail(result.ToErrorList());
            }

            if (await _unitOfWork.ProfileRepository.GetByUserIdAsync(userId) is not null)
                return Result.Fail(ProfileErrors.ProfileAlreadyExists);

            
            var tags = await _unitOfWork.TagRepository.GetByNames(request.Tags);

            Profile profile = new() {
                UserId = userId,
                Name= request.Name,
                BirthDate = request.BirthDate.ToDateTime(new TimeOnly()),
                Gender= request.Gender,
                Bio = request.Bio,
                City = request.City,
                Tags = tags,
                CreatedAt = DateTime.Now,
                LastActivity = DateTime.Now
            };
            
            await _unitOfWork.ProfileRepository.AddAsync(profile);
            await _unitOfWork.SaveAsync();
            


            return profile;
        }
        public async Task<Result<Profile>> UpdateProfile(int profileId ,ProfileFillRequest request)
        {
            var result = await _profileFillValidator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return Result.Fail(result.ToErrorList());
            }

            var profileByUserId = await _unitOfWork.ProfileRepository.GetWithTagsByIdAsync(profileId);
            if(profileByUserId == null)
                return Result.Fail(ProfileErrors.ProfileNotFound);

            var tags = await _unitOfWork.TagRepository.GetByNames(request.Tags);

            tags.RemoveAll((t) => profileByUserId.Tags.Any(x=>x.Id == t.Id));    

            profileByUserId.Name = request.Name;
            profileByUserId.BirthDate = request.BirthDate.ToDateTime(new TimeOnly());
            profileByUserId.Gender= request.Gender;
            profileByUserId.Bio = request.Bio;
            profileByUserId.City = request.City;
            profileByUserId.Tags.Clear();
            profileByUserId.Tags = tags;

            await _unitOfWork.SaveAsync();

            return profileByUserId;


        }

        public async Task<Result<Profile>> GetProfileByUsername(string login)
        {
            var profile = await _unitOfWork.ProfileRepository.GetByUsernameWithTagsAsync(login);

            if (profile is null)
                return Result.Fail(ProfileErrors.ProfileNotFound);

            return profile;

        }
        public async Task<Result<Profile>> GetProfileById(int profileId)
        {
            var profile = await _unitOfWork.ProfileRepository.GetWithTagsByIdAsync(profileId);

            if (profile is null)
                return Result.Fail(ProfileErrors.ProfileNotFound);

            return profile;

        }

        public async Task<Result<ProfileResult>> GetProfileWithRelationshipById(int profileId, int requesterId)
        {
            var profile = await _unitOfWork.ProfileRepository.GetWithTagsByIdAsync(profileId);

            if (profile is null)
                return Result.Fail(ProfileErrors.ProfileNotFound);
            
            return await GetProfileResult(profile, requesterId);
        }

        public async Task<Result<ProfileResult>> GetProfileWithRelationshipByUsername(string profileUsername, int requesterId)
        {
            var profile = await _unitOfWork.ProfileRepository.GetByUsernameWithTagsAsync(profileUsername);

            if (profile is null)
                return Result.Fail(ProfileErrors.ProfileNotFound);
            
            return await GetProfileResult(profile, requesterId);

        }

     
        private async Task<ProfileResult> GetProfileResult(Profile profile, int requesterId)
        {
            if (profile.Id == requesterId)
            {
                return _mapper.Map<ProfileResult>((profile, new CursorList<RelationshipProfile>()));
            }

            var relationship =
                await _unitOfWork.RelationshipRepository.GetRelationshipByProfileIdsAsync(profile.Id, requesterId);
            var mutualFriends =
                await _unitOfWork.ProfileRepository.GetMutualFriends(requesterId, profile.Id, 3);
            
            if(relationship is not null)
                return _mapper.Map<ProfileResult>((profile, relationship, mutualFriends));
            return _mapper.Map<ProfileResult>((profile, mutualFriends));
            
        }
        public async Task<Result<MediaFile>> UploadProfilePicture(int profileId, FileDto fileDto)
        {
            var valResult = await _profileImageValidator.ValidateAsync(fileDto);
            if (!valResult.IsValid)
            {
                return Result.Fail(valResult.ToErrorList());
            }
            
            var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(profileId);
            
            var now = DateTime.Now;
            var profilePicture = await _storageManager.UploadFileAsync(fileDto);
            
            await _unitOfWork.MediaFileRepository.AddAsync(profilePicture);
            profile!.MainPicture = profilePicture;
            

            await _unitOfWork.SaveAsync();

            return profilePicture;
        }

        public async Task<Result<CursorList<RelationshipProfileResult>>> GetProfilesByFilter(int profileId, CursorPaginationParams<DateTime> curParams, string searchQuery)
        {
            
            if(searchQuery.Length <= 3)
                return Result.Fail<CursorList<RelationshipProfileResult>>(ProfileErrors.SearchQueryTooShort);
            
            
            var profiles = await _unitOfWork
                .ProfileRepository.GetProfilesByFilter(profileId, curParams.PageSize, curParams.After, searchQuery);
            var profilesResult = _mapper.Map<CursorList<RelationshipProfileResult>>(profiles);
            return profilesResult;
        }
 
    }
}
