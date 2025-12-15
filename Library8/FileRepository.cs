using Library8.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library8.Models;
using Microsoft.EntityFrameworkCore;

namespace Library8
{
    public class FileRepository : Repository<Models.File>, IFileRepository
    {
        public readonly DBContext _context;
        public FileRepository(DBContext context) : base(context) {
        
            _context = context;
        }

        public async Task<IEnumerable<Models.File>> GetFilesByEntityAsync(int entityId, string entityType)
        {
            return await _context.Files
                .Where(f => f.RelatedEntityId == entityId && f.EntityType == entityType)
                .ToListAsync();
        }
        
    }

}
