namespace DotFlowControl;

public class ResultTypeBase<T>
{
    public T Value { get; private set; }

    protected ResultTypeBase(T value)
    {
        Value = value;
    }
}

public class Error<ERR> : ResultTypeBase<ERR>
{
    protected Error(ERR value) : base(value)
    {
    }

    public static Error<ERR> Build(ERR error)
    {
        return new Error<ERR>(error);
    }
}

public class Success<OK> : ResultTypeBase<OK>
{
    protected Success(OK value) : base(value)
    {
    }

    public static Success<OK> Build(OK ok)
    {
        return new Success<OK>(ok);
    }
    
}

public class Result<OK, ERR>
{
    public object Value { get; private set; }
    private Result(){}

    public static Result<OK, ERR> Success(OK success)
    {
        return new Result<OK, ERR>() {Value = Success<OK>.Build(success)};
    }

    public static Result<OK, ERR> Error(ERR error)
    {
        return new Result<OK, ERR>(){Value = Error<ERR>.Build(error)};
    }
}