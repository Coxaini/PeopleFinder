using PeopleFinder.Contracts.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Contracts.Recommendation
{
    public record RecommendationResponse(IEnumerable<ShortProfileResponse> Profiles);

}
