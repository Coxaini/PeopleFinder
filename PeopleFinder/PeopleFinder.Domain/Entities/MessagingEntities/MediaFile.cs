using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PeopleFinder.Domain.Entities.MessagingEntities
{
    public enum MediaFileType
    {
        Photo,
        Video,
        Audio
    }
    public class MediaFile
    {
        public long Id { get; set; }
        public Guid Token { get; set; }
        [MaxLength(10)]
        public string Extension { get; set; } = null!;
        public string OriginalName { get; set; } = null!;
        //public long MessageId { get; set; }
/*        public string? Token { get; set; }
        public DateTime? ExpirationDate { get; set; }*/
        public MediaFileType Type { get; set; }
        public DateTime UploadTime { get; set; }


    }
}
