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
        Image,
        Video,
        Audio
    }
    public class MediaFile
    {
       
        public Guid Id { get; set; }
        [MaxLength(10)]
        public string Extension { get; set; } = null!;
        public string OriginalName { get; set; } = null!;
        //public long MessageId { get; set; }
/*        public string? Token { get; set; }
        public DateTime? ExpirationDate { get; set; }*/
        public MediaFileType Type { get; set; }
        public DateTime UploadTime { get; set; }
        
        public bool IsPublic { get; set; }

    }
}
