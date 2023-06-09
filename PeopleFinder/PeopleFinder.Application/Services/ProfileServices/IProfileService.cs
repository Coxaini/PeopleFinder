﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;
using PeopleFinder.Application.Models.File;
using PeopleFinder.Application.Models.Profile;
using PeopleFinder.Domain.Common.Pagination.Cursor;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Application.Services.ProfileServices
{
    public interface IProfileService
    {
        Task<Result<Profile>> CreateProfile(int userId, ProfileEditRequest request);
        Task<Result<Profile>> UpdateProfile(int profileId, ProfileEditRequest request);
        Task<Result<Profile>> GetProfileByUsername(string login);
        Task<Result<Profile>> GetProfileById(int profileId);
        Task<Result<ProfileResult>> GetProfileWithRelationshipById(int profileId, int requesterId);
        Task<Result<ProfileResult>> GetProfileWithRelationshipByUsername(string profileUsername, int requesterId);
        Task<Result<MediaFile>> UploadProfilePicture(int profileId, FileDto fileDto);
        Task<Result<CursorList<RelationshipProfileResult>>> GetProfilesByFilter(int profileId ,CursorPaginationParams<DateTime> cursorPaginationParams, string searchQuery);
        Task<Result<Profile>> SetProfileOnline(int profileId);
        Task<Result<Profile>> SetProfileOffline(int profileId);
        
    }
}
