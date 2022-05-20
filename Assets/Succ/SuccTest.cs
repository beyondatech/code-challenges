using System;
using NUnit.Framework;

public class SuccTest
{
  [Test]
  public void ShouldPass()
  {
    Assert.That(Succ("AA"), Is.EqualTo("AB"));
    Assert.That(Succ("AZ"), Is.EqualTo("BA"));
    Assert.That(Succ("ZZ"), Is.EqualTo("AAA"));
    Assert.That(Succ("a9"), Is.EqualTo("b0"));
    Assert.That(Succ("z9"), Is.EqualTo("aa0"));
    Assert.That(Succ("9z"), Is.EqualTo("10a"));
  }

  string Succ(string value)
  {
    throw new NotImplementedException();
  }

}
