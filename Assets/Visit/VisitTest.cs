using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;

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

    /// <summary>
    /// Overload method for <see cref="Visit(VisitTest.INode)"/>, for passing on the tab string.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="tab"></param>
    /// <returns></returns>
    string Visit(INode root, string tab)
    {
        var sb = new StringBuilder();
        sb.Append(tab + root.Name); // Print root name 
        if (!Equals(root.Children, empty))
        {
            tab += "  "; // Indent with 2 spaces for hierarchy
            foreach (var child in root.Children)
            {
                sb.AppendLine(); // New line foreach child
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
        public IEnumerable<INode> Children => _orderedChildren;
        private List<INode> _orderedChildren;
        public IEnumerable<INode> DependsOn => _dependsOn;
        private HashSet<INode> _dependsOn;

        public Node(string name, IEnumerable<INode> children, IEnumerable<INode> dependsOn)
        {
            Name = name;
            _dependsOn = new HashSet<INode>(dependsOn);
            foreach (var node in _dependsOn)
            {
                if (node.DependsOn.Contains(this))
                    Debug.LogWarning($"Circular dependency between {Name} and {node.Name}!");
                if (reverseDependency.ContainsKey(node))
                {
                    reverseDependency[node].Add(this);
                }
                else
                {
                    reverseDependency.Add(node, new HashSet<INode>() { this });
                }
            }

            var distinctChildren = new HashSet<INode>();
            foreach (var child in children)
            {
                distinctChildren.Add(child);
                if (!reverseDependency.ContainsKey(child)) continue;
                foreach (var node in reverseDependency[child])
                {
                    distinctChildren.Add(node);
                }
            }

            // Todo : re-sort empty dependence elements because OrderBy() isn't comparing each node, could be optimised
            var tail = distinctChildren.Where(c => c.DependsOn.Any()).OrderBy(c => c).ToList();
            var head = distinctChildren.Where(c => !c.DependsOn.Any()).ToList();
            head.AddRange(tail);
            _orderedChildren = head;
        }

        public int CompareTo(object obj)
        {
            if (obj is not Node node) throw new InvalidCastException("This node can only compare with its own type!");

            if (DependsOn.Contains(node))
                return 1; // This instance follows obj in the sort order.

            if (node.DependsOn.Contains(this))
                return -1; // This instance precedes obj in the sort order.

            return 0; // This instance occurs in the same position in the sort order as obj.
        }

        /// <summary>
        /// For storing reverse dependency
        /// </summary>
        private static Dictionary<INode, HashSet<INode>> reverseDependency = new();
    }
}