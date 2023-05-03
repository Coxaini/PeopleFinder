using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Services.Authorization
{
    public interface IAccessVerificationService
    {
        public bool IsUserHasAccess(int userId);
    }
}
