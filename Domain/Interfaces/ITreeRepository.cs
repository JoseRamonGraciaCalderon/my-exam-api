using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITreeRepository
    {
        Task<List<TreeNode>> GetTreeAsync();
        Task<TreeNode> GetNodeByIdAsync(string id);
        Task AddNodeAsync(TreeNode node);
        Task UpdateNodeAsync(TreeNode node);
        Task DeleteNodeAsync(string id);
    }
}
