using System;
using System.Collections.Generic;
using System.Linq;
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
    Assert.That(Visit(ANode("Ground", children: new[] { chair, desk, bed }, dependsOn: empty)), Is.EqualTo(expect));
    Assert.That(Visit(ANode("Ground", children: new[] { chair, bed, desk }, dependsOn: empty)), Is.EqualTo(expect));
  }

  string Visit(INode root) => throw new NotImplementedException();

  INode ANode(string name, IEnumerable<INode> children, IEnumerable<INode> dependsOn)
    => throw new NotImplementedException();

  interface INode
  {
    string Name { get; }

    IEnumerable<INode> Children { get; }

    IEnumerable<INode> DependsOn { get; }
  }


}
