using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

    /// <summary>
    /// Recursive function for process string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string Succ(string value)
    {
        if (value.Length > 1)
        {
            var first = value.First();
            var rest = value[1..];
            var result = Succ(rest);
            if (result.First().Equals('A') || result.First().Equals('a') || result.First().Equals('0'))
            {
                return ToNextInit(first) + result;
            }
            else
            {
                return first + result;
            }
        }
        else
        {
            var first = value.First();
            return ToNext(first);
        }
    }

    /// <summary>
    /// Loop character from A-Z/a-zm handle number differently, ie: 9 -> 0
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string ToNext(char value)
    {
        if (char.IsNumber(value))
        {
            var number = Convert.ToInt32($"{value}");
            var output = number < 9 ? number + 1 : 0;
            return $"{output}";
        }
        else
        {
            if (char.IsLower(value))
            {
                var output = value.Equals('z') ? 'a' : Convert.ToChar(Convert.ToInt32(value) + 1);
                return $"{output}";
            }
            else
            {
                var output = value.Equals('Z') ? 'A' : Convert.ToChar(Convert.ToInt32(value) + 1);
                return $"{output}";
            }
        }
    }

    /// <summary>
    /// Initial add one if run by these particular cases 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string ToNextInit(char value)
    {
        var v = ToNext(value);
        return v switch
        {
            "0" => "10",
            "a" => "aa",
            "A" => "AA",
            _ => v
        };
    }
}