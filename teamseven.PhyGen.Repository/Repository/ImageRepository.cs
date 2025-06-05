using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamseven.PhyGen.Repository.Basic;
using teamseven.PhyGen.Repository.Models;

namespace teamseven.PhyGen.Repository.Repository
{
    public class ImageRepository : GenericRepository<Image>
    {
        private readonly teamsevenphygendbContext _context;
        public ImageRepository() { }

        public ImageRepository(teamsevenphygendbContext context)
        {
            _context = context;
        }

        public async Task<Image?> GetImageByIdAsync(int id)
        {
            try
            {
                return await _context.Images.FirstOrDefaultAsync(x => x.ImageId == id);
            }
            catch (Exception ex)
            { 
                throw new Exception($"Error retrieving image with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task AddImageAsync(Image image) =>           await CreateAsync(image);

        public async Task<IEnumerable<Image?>> GetImageInListAsync(string id)
        {
            try
            {
                // check null or white space
                if (string.IsNullOrWhiteSpace(id))
                {
                    return Enumerable.Empty<Image?>();
                }

                // split into List
                List<int> imageListCode = id.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(x => int.TryParse(x.Trim(), out int result) ? result : -1)
                                           .Where(x => x != -1)
                                           .ToList();

                // if null return empty
                if (!imageListCode.Any())
                {
                    return Enumerable.Empty<Image?>();
                }

                // query 
                var images = await _context.Images
                    .Where(x => imageListCode.Contains(x.ImageId))
                    .ToListAsync();

                //return result
                var result = imageListCode
                    .Select(id => images.FirstOrDefault(x => x.ImageId == id))
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving images for IDs '{id}': {ex.Message}", ex);
            }
        }

        public async Task<int> AddImageAndReturnPK(Image image)
        {
            try
            {
               
                if (image == null)
                    throw new ArgumentNullException(nameof(image));

               
                await _context.Images.AddAsync(image);

                
                await _context.SaveChangesAsync();

                // khoá chính có ID thực nhờ sức mạnh của savechanges async
                return image.ImageId;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error adding image: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding image: {ex.Message}", ex);
            }
        }
    }
}
