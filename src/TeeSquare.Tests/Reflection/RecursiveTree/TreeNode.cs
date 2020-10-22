using System.Collections.Generic;

namespace TeeSquare.Tests.Reflection.TreeDomain
{
    public class TreeNode
    {
        public string Id { get; set; }
        public List<TreeNode> Children { get; set; }
    }
}
