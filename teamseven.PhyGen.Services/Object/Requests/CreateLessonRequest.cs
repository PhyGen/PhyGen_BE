﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Services.Object.Requests
{
    public class CreateLessonRequest
    {
        [Required(ErrorMessage = "Lesson name is required.")]
        [StringLength(255, ErrorMessage = "Lesson name must be at most 255 characters.")]
        public string Name { get; set; }

        public int ChapterId { get; set; }
    }
}
