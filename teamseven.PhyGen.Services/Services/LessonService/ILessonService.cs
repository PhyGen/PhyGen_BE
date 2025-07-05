﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.LessonService
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDataResponse>> GetAllLessonAsync();

        Task<PagedResponse<LessonDataResponse>> GetLessonsAsync(
            int? pageNumber = null,
               int? pageSize = null,
               string? search = null,
               string? sort = null,
               int? chapterId = null,
               int isSort = 0);

        Task<LessonDataResponse> GetLessonByIdAsync(int id);
        Task CreateLessonAsync(CreateLessonRequest request);
        Task UpdateLessonAsync(LessonDataRequest request);
        Task DeleteLessonAsync(int id);
    }
}
