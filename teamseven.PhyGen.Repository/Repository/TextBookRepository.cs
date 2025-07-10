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
    public class TextBookRepository : GenericRepository<TextBook>
    {
        private readonly teamsevenphygendbContext _context;

        public TextBookRepository(teamsevenphygendbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<TextBook>> GetAllTextBooksAsync()
        {
            return await _context.TextBooks.ToListAsync();
        }

        public async Task<TextBook?> GetTextBookByIdAsync(int id)
        {
            return await _context.TextBooks.FindAsync(id);
        }

        public async Task CreateTextBookAsync(TextBook textBook)
        {
            if (textBook == null)
            {
                throw new ArgumentNullException(nameof(textBook));
            }

            _context.TextBooks.Add(textBook);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTextBookAsync(TextBook textBook)
        {
            if (textBook == null)
            {
                throw new ArgumentNullException(nameof(textBook));
            }

            _context.TextBooks.Update(textBook);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTextBookAsync(int id)
        {
            var textBook = await GetTextBookByIdAsync(id);
            if (textBook == null)
            {
                throw new KeyNotFoundException($"TextBook with ID {id} not found.");
            }

            _context.TextBooks.Remove(textBook);
            await _context.SaveChangesAsync();
        }
    }
}
