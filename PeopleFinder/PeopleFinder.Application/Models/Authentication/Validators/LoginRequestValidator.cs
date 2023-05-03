using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Models.Authentication.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.EmailOrUsername).MinimumLength(4).WithMessage("Minimum length is 5");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Invalid password");

        }

    }
}
