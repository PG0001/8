using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library8.Models.Interfaces
{
    public interface IFileRepository : IRepository<File>
    {
        Task AddAsync(File file);
        Task<IEnumerable<Models.File>> GetFilesByEntityAsync(int entityId, string entityType);
        Task SaveChangesAsync();
    }

}
