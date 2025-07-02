using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Services.Services.SolutionLinkService
{
    public class SolutionLinkService : ISolutionLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SolutionLinkService> _logger;

        public SolutionLinkService(IUnitOfWork unitOfWork, ILogger<SolutionLinkService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AddAsync(SolutionLinkRequest request)
        {
            var solution = await _unitOfWork.SolutionRepository.GetByIdAsync(request.SolutionId);
            if (solution == null)
                throw new KeyNotFoundException($"Solution {request.SolutionId} not found.");

            var entity = new SolutionsLink
            {
                SolutionId = request.SolutionId,
                Link = request.Link,
                GeneratedBy = request.GeneratedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.SolutionsLinkRepository.CreateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task<SolutionLinkResponse> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.SolutionsLinkRepository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"SolutionLink {id} not found.");

            return new SolutionLinkResponse
            {
                Id = entity.Id,
                SolutionId = entity.SolutionId,
                Link = entity.Link,
                GeneratedBy = entity.GeneratedBy,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public async Task<List<SolutionLinkResponse>> GetBySolutionIdAsync(int solutionId)
        {
            var list = await _unitOfWork.SolutionsLinkRepository.GetBySolutionIdAsync(solutionId);
            return list.Select(x => new SolutionLinkResponse
            {
                Id = x.Id,
                SolutionId = x.SolutionId,
                Link = x.Link,
                GeneratedBy = x.GeneratedBy,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
        }

        public async Task UpdateAsync(int id, SolutionLinkRequest request)
        {
            var entity = await _unitOfWork.SolutionsLinkRepository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"SolutionLink {id} not found.");

            entity.Link = request.Link;
            entity.GeneratedBy = request.GeneratedBy;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SolutionsLinkRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.SolutionsLinkRepository.GetByIdAsync(id);
            if (entity != null)
            {
                await _unitOfWork.SolutionsLinkRepository.RemoveAsync(entity);
                await _unitOfWork.SaveChangesWithTransactionAsync();
            }
        }
    }
}
