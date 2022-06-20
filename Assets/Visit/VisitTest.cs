using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

public class VisitTest
{
    readonly IEnumerable<INode> empty = Enumerable.Empty<INode>();

    [Test]
    public void Sequence()
    {
        var expect =
            @"Ground
  Bed
  Desk
    Light
  Chair";

        var bed = ANode("Bed", children: empty, dependsOn: empty);
        var light = ANode("Light", children: empty, dependsOn: empty);
        var desk = ANode("Desk", children: new[] { light }, dependsOn: new[] { bed });
        var chair = ANode("Chair", children: empty, dependsOn: new[] { desk });

        Assert.That(Visit(ANode("Ground", children: new[] { bed, desk, chair }, dependsOn: empty)), Is.EqualTo(expect));
        Assert.That(Visit(ANode("Ground", children: new[] { desk, desk, bed }, dependsOn: empty)), Is.EqualTo(expect));
        Assert.That(Visit(ANode("Ground", children: new[] { chair, bed, desk }, dependsOn: empty)), Is.EqualTo(expect));
    }

    string Visit(INode root)
    {
        var sb = new StringBuilder();
        var tab = string.Empty;
        sb.Append(Visit(root, tab));
        return sb.ToString();
    }

    string Visit(INode root, string tab)
    {
        var sb = new StringBuilder();
        sb.Append(tab + root.Name); // Print root name 
        if (!Equals(root.Children, empty))
        {
            tab += "  "; // Indent with 2 spaces for hierarchy
            foreach (var child in root.Children.OrderBy(c => c))
            {
                sb.AppendLine();
                sb.Append(Visit(child, tab));
            }
        }

        return sb.ToString();
    }

    INode ANode(string name, IEnumerable<INode> children, IEnumerable<INode> dependsOn)
        => new Node(name, children, dependsOn);

    interface INode
    {
        string Name { get; }

        IEnumerable<INode> Children { get; }

        IEnumerable<INode> DependsOn { get; }
    }

    class Node : INode, IComparable
    {
        public string Name { get; }
        public IEnumerable<INode> Children => _children;
        private HashSet<INode> _children;
        public IEnumerable<INode> DependsOn => _dependsOn;
        private HashSet<INode> _dependsOn;

        public Node(string name, IEnumerable<INode> children, IEnumerable<INode> dependsOn)
        {
            Name = name;
            _children = new HashSet<INode>(children);
            _dependsOn = new HashSet<INode>(dependsOn);
            //reverseDependency.ContainsKey()
        }

        public int CompareTo(object obj)
        {
            if (obj is not Node node) throw new InvalidCastException("This node can only compare with its own type!");
            if (_dependsOn.Contains(node)) return 1; //	This instance follows obj in the sort order.
            else if (node._dependsOn.Contains(this)) return -1; // This instance precedes obj in the sort order.
            else return 0; // This instance occurs in the same position in the sort order as obj.
        }
        
        // To create reverse logic link from dependency
        private static Dictionary<INode, IEnumerable<INode>> reverseDependency = new Dictionary<INode, IEnumerable<INode>>();
    }
}