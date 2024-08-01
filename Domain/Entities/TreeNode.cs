namespace Domain.Entities
{
    public class TreeNode
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();
    }
}
