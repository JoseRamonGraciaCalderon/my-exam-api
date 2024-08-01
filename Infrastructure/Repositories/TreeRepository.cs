using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class TreeRepository : ITreeRepository
    {
        private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "tree.json");

        public async Task<List<TreeNode>> GetTreeAsync()
        {
            if (!File.Exists(_jsonFilePath))
            {
                return new List<TreeNode>();
            }

            var json = await File.ReadAllTextAsync(_jsonFilePath);
            return JsonSerializer.Deserialize<List<TreeNode>>(json);
        }

        public async Task<TreeNode> GetNodeByIdAsync(string id)
        {
            var tree = await GetTreeAsync();
            return FindNodeById(tree, id);
        }

        public async Task AddNodeAsync(TreeNode node)
        {
            var tree = await GetTreeAsync();
            tree.Add(node);
            await SaveTreeAsync(tree);
        }

        public async Task UpdateNodeAsync(TreeNode node)
        {
            var tree = await GetTreeAsync();
            var existingNode = FindNodeById(tree, node.Id);
            if (existingNode != null)
            {
                existingNode.Name = node.Name;
                existingNode.Children = node.Children ?? new List<TreeNode>();

                // Generar ID para los nuevos hijos que no tienen ID
                foreach (var child in existingNode.Children)
                {
                    if (string.IsNullOrEmpty(child.Id))
                    {
                        child.Id = Guid.NewGuid().ToString();
                    }
                }

                await SaveTreeAsync(tree);
            }
        }
        public async Task DeleteNodeAsync(string id)
        {
            var tree = await GetTreeAsync();
            if (DeleteNodeById(tree, id))
            {
                await SaveTreeAsync(tree);
            }
        }

        private async Task SaveTreeAsync(List<TreeNode> tree)
        {
            var json = JsonSerializer.Serialize(tree, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }

        private TreeNode FindNodeById(List<TreeNode> nodes, string id)
        {
            foreach (var node in nodes)
            {
                if (node.Id == id) return node;

                var found = FindNodeById(node.Children, id);
                if (found != null) return found;
            }

            return null;
        }

        private bool DeleteNodeById(List<TreeNode> nodes, string id)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Id == id)
                {
                    nodes.RemoveAt(i);
                    return true;
                }

                if (DeleteNodeById(nodes[i].Children, id))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
