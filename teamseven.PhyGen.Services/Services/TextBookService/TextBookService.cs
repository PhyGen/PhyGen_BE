﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Object.Responses;

namespace teamseven.PhyGen.Services.Services.TextBookService
{
    public class TextBookService : ITextBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TextBookService> _logger;

        public TextBookService(IUnitOfWork unitOfWork, ILogger<TextBookService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<TextBookDataResponse>> GetAllTextBookAsync()
        {
            var textbooks = await _unitOfWork.TextBookRepository.GetAllAsync();
            return textbooks.Select(tb => new TextBookDataResponse
            {
                Id = tb.Id,
                Name = tb.Name,
                GradeId = tb.GradeId,
                CreatedAt = tb.CreatedAt,
                UpdatedAt = tb.UpdatedAt
            });
        }

        public async Task<TextBookDataResponse> GetTextBookByIdAsync(int id)
        {
            var tb = await _unitOfWork.TextBookRepository.GetByIdAsync(id);
            if (tb == null)
                throw new NotFoundException($"Textbook with ID {id} not found.");

            return new TextBookDataResponse
            {
                Id = tb.Id,
                Name = tb.Name,
                GradeId = tb.GradeId,
                CreatedAt = tb.CreatedAt,
                UpdatedAt = tb.UpdatedAt
            };
        }

        public async Task CreateTextBookAsync(CreateTextBookRequest request)
        {
            var textbook = new TextBook
            {
                Name = request.Name,
                GradeId = request.GradeId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.TextBookRepository.CreateAsync(textbook);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task UpdateTextBookAsync(TextBookDataRequest request)
        {
            var existing = await _unitOfWork.TextBookRepository.GetByIdAsync(request.Id);
            if (existing == null)
                throw new NotFoundException($"Textbook with ID {request.Id} not found.");

            existing.Name = request.Name;
            existing.GradeId = request.GradeId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.TextBookRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesWithTransactionAsync();
        }

        public async Task DeleteTextBookAsync(int id)
        {
            var textbook = await _unitOfWork.TextBookRepository.GetByIdAsync(id);
            if (textbook == null)
                throw new NotFoundException($"Textbook with ID {id} not found.");

            await _unitOfWork.TextBookRepository.RemoveAsync(textbook);
            await _unitOfWork.SaveChangesWithTransactionAsync();

        }
    }
}
