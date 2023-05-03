
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using PeopleFinder.Application.Common.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static List<ValidationError> ToErrorList(this ValidationResult result)
        {
            return result.Errors.ConvertAll(error => new ValidationError(error.PropertyName, error.ErrorMessage));

        }

    }
}
