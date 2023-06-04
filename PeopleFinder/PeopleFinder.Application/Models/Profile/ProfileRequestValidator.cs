using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Application.Common.Extensions.CustomValidators;
using PeopleFinder.Domain.Repositories.Common;

namespace PeopleFinder.Application.Models.Profile
{

    public class ProfileCreateRequestValidator : AbstractValidator<ProfileEditRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProfileCreateRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            
            RuleFor(x => x.Name).MustBeAValidName();

            RuleFor(x => x.BirthDate.Value)
                .MustBeAValidBirthDate()
                .When(x => x.BirthDate.HasValue);

            RuleFor(x => x.Gender).Cascade(CascadeMode.Stop).MustBeAValidGender();
            RuleFor(x => x.Username).Cascade(CascadeMode.Stop).MinimumLength(5).MaximumLength(30);
            RuleFor(x => x.Bio).MaximumLength(400);
            RuleFor(x => x.City).MaximumLength(40);

            RuleFor(x => x.Tags).Cascade(CascadeMode.Stop).
                Must(x => x.Count <= 6).
                WithMessage("No more than 6 tags are allowed").
                ForEach(tag =>
                {
                    tag.NotEmpty().MustAsync(IsTagNameExistsInADb).WithName(x => $"{x}").WithMessage("tag doesn't exists");
                });
        }
        
        private async Task<bool> IsTagNameExistsInADb(string tag, CancellationToken token)
        {
            if(await _unitOfWork.TagRepository.GetFirstOrDefault(x => x.Name == tag) is null) return false;
            return true;
        }

    }

    /*public class ProfileEditRequestValidator : AbstractValidator<ProfileEditRequest>
    {
        public ProfileEditRequestValidator()
        {
            RuleFor(x => x.Name).MustBeAValidName();

            RuleFor(x => x.BirthDate).MustBeAValidBirthDate();

            RuleFor(x => x.Gender).IsInEnum().MustBeAValidGender();
            RuleFor(x => x.GenderInterest).IsInEnum().MustBeAValidMultipleGender();
            RuleFor(x => x.Tags.Count).GreaterThan(0).LessThan(6);

        }
    }*/


}
