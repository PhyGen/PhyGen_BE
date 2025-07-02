﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Services.Object.Responses
{
    public class LessonDataResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChapterId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
