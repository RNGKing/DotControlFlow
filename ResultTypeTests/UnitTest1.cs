using System;
using System.Collections.Generic;
using System.Diagnostics;
using DotFlowControl;
using NUnit.Framework;

namespace ResultTypeTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSuccessState()
    {
        var result = Result<object, string>.Success(new object());
        var goodTest = false;
        switch (result.Value)
        {
            case Success<object> success:
                goodTest = true;
                break;
            default:
                break;
        }
        Assert.IsTrue(goodTest);
    }
    [Test]
    public void TestErrorState()
    {
        var result = Result<object, object>.Error(new object());
        var goodTest = false;
        switch (result.Value)
        {
            case Error<object> error:
                goodTest = true;
                break;
            default:
                break;
        }
        Assert.IsTrue(goodTest);
    }
    [Test]
    public void TestPerformance()
    {
        uint lowIteration = 10;
        uint moderateIteraion = 1000;
        uint BurdonsomeIteration = 100000;
        uint veryBurdonSomeIteration = 1000000000;
        RunAndOutputTest("Low Iteration" , lowIteration);
        RunAndOutputTest("Moderate Iteration", moderateIteraion);
        RunAndOutputTest("Burdonsome Iteration", BurdonsomeIteration);
        RunAndOutputTest("Very Burdonsome Iteration", veryBurdonSomeIteration);
    }

    /// <summary>
    /// Generates a 
    /// </summary>
    private IEnumerable<Result<object, string>> BasicTest(uint count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return Result<object, string>.Success(new object());
        }
    }

    private void RunAndOutputTest(string typeOfTest, uint count)
    {
        Stopwatch watch = Stopwatch.StartNew();
        foreach (var iteration in BasicTest(count))
        {
            switch (iteration.Value)
            {
                case Success<object> success:
                    break;
                case Error<string> error:
                    break;
            }
        }
        Console.WriteLine($"{typeOfTest} : {watch.ElapsedMilliseconds}ms");
    }
    
}