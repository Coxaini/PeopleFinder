using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleFinder.Domain.Entities.MessagingEntities;

namespace PeopleFinder.Domain.Entities
{
    public class ProfilePicture
    {
        public long Id { get; set; }
        public Guid PictureId { get; set; }
        public MediaFile? Picture { get; set; }
        public Profile? Profile { get; set; }
    }
}
