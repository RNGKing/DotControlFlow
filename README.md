# DotControlFlow
A type helper library to make more complicated interactions easier to read and comprehend.
# Reasoning
As complexity of code increases, the amount of things that methods and 
functions are intended to do increases, the amount of different return types,
data types and custom interactions builds. This library is intended to help
wrangle some of that complexity and make it more clear as to what is happening
inside of a codebase.

This library takes heavy inspiration from F# and Rust. If I could figure
out how to convince Microsoft to add this as a standard feature, I would.

# Old Way

Here's an example of one way of doing error based function handling:

```
public bool Foo(out string message)
{
    message = string.Empty;
    // Does something
    // oh no it failed!
    message = "FOO FAILED!";
    return false;
}
```
You could use error codes to indicate the problem, but that can obfuscate
and make it hard to understand what's happening in my opinion:

```
public ERROR_CODE Foo(out string message)
{
    message = string.Empty;
    // Does something
    // Oh no it failed!
    message = "FOO FAILED!";
    return ERROR_CODE.FAIL;
}
```

Using this way requires you to then have an additional function which will then
parse the Error code and turn it into a meaninful response.

Now don't even get me started on what happens when you have to do something
that creates or modifies data, it gets even uglier!

```
public ERROR_CODE Foo(out BARTYPE bar, out string message)
{
    bar = default;
    message = string.Empty;
    // Does something
    // Oh no it failed!
    message = "FOO FAILED!";
    return ERROR_CODE.FAIL;
}
```

The reference type could be null, it could be mutated oddly, there are just
lots of questions that pop up in my head when I see something like this.

#The DotControlFlow Way

We move the needed types to the front of the method, we no longer have
hanging strange out parameters and as an added benefit, you should compose
the functions that use this in a functional way that doesn't lead to side
effects that may gum up the works later!

```
public Result<BARTYPE, string> Foo()
{
    // DOES SOMETHING
    // OH NO IT FAILED!
    return Result<BARTYPE, string.Error("Foo Failed!");
}
```

# Basic Usage Example

```
public struct Data
{
    public int Foo;
    public int Bar;
    public string Rev;
    
    public Data(int foo, int bar, string rev)
    {
        Foo = foo;
        Bar = bar;
        Rev = rev;
    }
}

public static class DataLoader 
{
    public Result<Data, string> LoadData(string filePath)
    {
        if(!File.Exists(filePath))
            return Result<Data,string>.Error($"File at {filePath} doesn't exist!");
        try
        {
            var lines = File.ReadLines(filePath);
            if(!int.TryParse(lines[0], out int foo)
                return Result<Data, string>.Error("First line is not an integer value!");
            if(!int.TryParse(lines[1], out int bar)
                return Result<Data, string>.Error("Second line is not an integer value!");
            var rev = lines[2];
            return Result<Data, string>.Success(new Data(foo, bar, rev));
        }
        catch(Exception er)
        {
            return Result<Data, string>.Error(er.Message);
        }
    }
    
    public static Result<object, string> CallingMethod()
    {
        var path = "hard/coded/path.txt"
        LoadData(path)
            .Match( 
                success : result => Console.WriteLine($"Rev : {result.Rev}),
                error : Error(error => Console.WriteLine(error)
            );
    }
}


```

#Some Considerations
It should be noted, that this library is just my opinion on how code should 
look and flow. If you have simple <Yes/No> success states and uncomplicated
error handling, then returning a boolean to indicate success is fine in my
opinion. However, once you start getting in the weeds of more complicated interactions
like DB access, live camera feed access, PLC interactions or even the simple
File I/O you can start running into problems, fast.
