using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library8.Models.Interfaces
{
    public interface ITaskTimelineRepository
    {
        Task AddAsync(TaskTimeline timeline);
        Task<IEnumerable<TaskTimeline>> GetByTaskIdAsync(int taskId);
        Task SaveChangesAsync();
    }
}
