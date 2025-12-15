using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library8.Models.Interfaces
{
    public interface ITaskCommentRepository
    {
        Task AddAsync(TaskComment comment);
        Task<IEnumerable<TaskComment>> GetByTaskIdAsync(int taskId);
        Task SaveChangesAsync();
    }
}
