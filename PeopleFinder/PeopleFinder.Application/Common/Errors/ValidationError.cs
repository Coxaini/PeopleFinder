using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Common.Errors
{
    public class ValidationError : Error
    {
        public ValidationError(string code, string message) : base( message)
        {   
            Metadata.Add("Code", code );
            Metadata.Add("Type", ErrorType.Validation);
        }
    }
}
