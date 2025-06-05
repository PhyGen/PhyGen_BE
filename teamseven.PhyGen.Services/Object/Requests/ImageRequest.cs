using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Services.Object.Requests
{
    public class ImageRequest
    {
        public required string ImageUrl { get; set; }

        public string? Details { get; set; }
    }
}
