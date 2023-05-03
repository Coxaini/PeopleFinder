using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Models.Authentication.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x=>x.Username).NotEmpty().MinimumLength(5).WithMessage("Login minimum length is 5")
                .Matches(@"^(?=.*[A-Za-z])[A-Za-z0-9]+$")
                .WithMessage("Login must contain at least one letter and can contain only latin letters and numbers");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Password minimum length is 8");

        }

    }

}
