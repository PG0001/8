using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library8.Models.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetByUserAsync(int userId);
        Task<Notification?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }


}
