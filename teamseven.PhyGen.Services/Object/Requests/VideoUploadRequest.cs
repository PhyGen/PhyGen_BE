using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Services.Object.Requests
{
    public class VideoUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }

}
