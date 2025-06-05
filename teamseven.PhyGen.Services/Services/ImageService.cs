using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository.Repository;
using teamseven.PhyGen.Services.Interfaces;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Extensions;
using teamseven.PhyGen.Services.MappingConfig;
using Mapster;

namespace teamseven.PhyGen.Services.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageRepository _imageRepository;

        public ImageService(ImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<Image> GetImageAsync(int id)
        {
            if (id < 0) throw new ArgumentNullException("Invalid or null id");
            //get id
            var img = await _imageRepository.GetByIdAsync(id);

            if (img == null) throw new KeyNotFoundException("Not found ID");

            else return img;

        }

        public async Task<string> GetURLByIDAsync(int id)
        {
            if (id < 0) throw new ArgumentNullException("Invalid or null id");
            //get id
            var img = await _imageRepository.GetByIdAsync(id);

            if (img == null) throw new KeyNotFoundException("Not found ID");

            else return img.ImageUrl;
        }

        public async Task SaveImageAsync(ImageRequest imageRequest)
        {
            try
            {

                //validation
                if (imageRequest == null)
                    throw new ArgumentNullException(nameof(imageRequest));

                if (!ValidateInputService.IsNotEmpty(imageRequest.ImageUrl)) throw new KeyNotFoundException("Image URL is required");

                // Ánh xạ ImageRequest sang Image
                var image = imageRequest.Adapt<Image>();
                image.UploadedAt = DateTime.UtcNow; 

                // Tạo bản ghi và trả về ImageId
                await _imageRepository.CreateReturnKeyAsync<int>(image);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving image: {ex.Message}", ex);
            }
        }
    }
}
