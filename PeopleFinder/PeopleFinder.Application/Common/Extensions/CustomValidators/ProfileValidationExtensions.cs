using FluentValidation;
using PeopleFinder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Common.Extensions.CustomValidators
{
    public static class ProfileValidationExtensions
    {
        public static IRuleBuilderOptions<T, Gender> MustBeAValidGender<T>(this IRuleBuilder<T, Gender> ruleBuilder)
        {
            
            return ruleBuilder.IsInEnum().WithMessage("Gender must be either None, Male or Female");
        }
        public static IRuleBuilderOptions<T, Gender> MustBeAValidMultipleGender<T>(this IRuleBuilder<T, Gender> ruleBuilder)
        {
            return ruleBuilder.IsInEnum()
                             .WithMessage("Incorrect gender value");
        }

        public static IRuleBuilderOptions<T, string> MustBeAValidName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().WithMessage("Name is required.")
                .MinimumLength(4).WithMessage("Name must be at least 4 characters long.")
                .MaximumLength(30).WithMessage("Name cannot exceed 30 characters.");
        }

        public static IRuleBuilderOptions<T, DateOnly> MustBeAValidBirthDate<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
        {
            return ruleBuilder.Must(d => d < DateOnly.FromDateTime(DateTime.Today)).
                WithMessage("Birth date must be in the past and not greater than today's date");
        }

    }
}
