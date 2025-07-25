using Microsoft.Extensions.Logging;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Services.OtherServices;

namespace teamseven.PhyGen.Services.Services.SolutionService
{
    public class SolutionService : ISolutionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SolutionService> _logger;
        private readonly SupabaseService _supabaseService;

        public SolutionService(IUnitOfWork unitOfWork, ILogger<SolutionService> logger, SupabaseService supabaseService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _supabaseService = supabaseService;
        }

        public async Task<IEnumerable<SolutionDataResponse>> GetAllSolutionsAsync()
        {
            var solutions = await _unitOfWork.SolutionRepository.GetAllAsync();
            return solutions.Select(s => new SolutionDataResponse
            {
                Id = s.Id,
                QuestionId = s.QuestionId,
                Content = s.Content,
                Explanation = s.Explanation,
                CreatedByUserId = s.CreatedByUserId,
                IsApproved = s.IsApproved,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                VideoData = s.VideoData,
                VideoContentType = s.VideoContentType
            });
        }

        public async Task<SolutionDataResponse> GetSolutionByIdAsync(int id)
        {
            var solution = await _unitOfWork.SolutionRepository.GetByIdAsync(id);
            if (solution == null)
                throw new NotFoundException($"Solution with ID {id} not found.");

            return new SolutionDataResponse
            {
                Id = solution.Id,
                QuestionId = solution.QuestionId,
                Content = solution.Content,
                Explanation = solution.Explanation,
                CreatedByUserId = solution.CreatedByUserId,
                IsApproved = solution.IsApproved,
                CreatedAt = solution.CreatedAt,
                UpdatedAt = solution.UpdatedAt,
                VideoData = solution.VideoData,
                VideoContentType = solution.VideoContentType
            };
        }

        public async Task CreateSolutionAsync(CreateSolutionRequest request)
        {
            var solution = new Solution
            {
                QuestionId = request.QuestionId,
                Content = request.Content,
                Explanation = request.Explanation,
                CreatedByUserId = request.CreatedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.SolutionRepository.CreateAsync(solution);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task UpdateSolutionAsync(SolutionDataRequest request)
        {
            var solution = await _unitOfWork.SolutionRepository.GetByIdAsync(request.Id);
            if (solution == null)
                throw new NotFoundException("Solution not found.");

            solution.Content = request.Content;
            solution.Explanation = request.Explanation;
            solution.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SolutionRepository.UpdateAsync(solution);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task DeleteSolutionAsync(int id)
        {
            var solution = await _unitOfWork.SolutionRepository.GetByIdAsync(id);
            if (solution == null)
                throw new NotFoundException("Solution not found.");

            await _unitOfWork.SolutionRepository.RemoveAsync(solution);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }
        public async Task AddSolutionWithVideoAsync(SolutionWithVideoRequest request)
        {
            //string videoUrl = null;

            //if (request.VideoFile != null)
            //{
            //    // Upload lên Supabase và lấy URL
            //    videoUrl = await _supabaseService.UploadVideoAsync(request.VideoFile);
            //}

            //var solution = new Solution
            //{
            //    QuestionId = request.QuestionId ?? -1,
            //    Content = request.Content,
            //    CreatedByUserId = 1,
            //    CreatedAt = DateTime.UtcNow,
            //    VideoData = videoUrl // lưu URL thay vì byte[]
            //,
            //    VideoContentType = request.VideoFile?.ContentType ?? "video/mp4", 
            //};

            //try
            //{
            //    await _unitOfWork.SolutionRepository.AddAsync(solution);
            //    await _unitOfWork.SaveChangesWithTransactionAsync();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Lỗi khi thêm Solution");
            //    throw; 
            //}

        }
        public async Task UpdateSolutionVideoAsync(SolutionWithVideoRequest request)
        {
            var solution = await _unitOfWork.SolutionRepository.GetByIdAsync(request.SolutionId);
            if (solution == null)
                throw new NotFoundException("Solution not found.");

            if (request.VideoFile != null)
            {
                var videoUrl = await _supabaseService.UploadVideoAsync(request.VideoFile);

                solution.Mp4Url = videoUrl;
                solution.VideoContentType = request.VideoFile.ContentType ?? "video/mp4";
                solution.IsMp4Generated = true;
                solution.UpdatedAt = DateTime.UtcNow;
            }

            if (!string.IsNullOrEmpty(request.Content))
                solution.Content = request.Content;

            if (!string.IsNullOrEmpty(request.VideoData))
                solution.VideoData = request.VideoData;

            await _unitOfWork.SolutionRepository.UpdateAsync(solution);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }
    }
}
