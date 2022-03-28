namespace DotFlowControl;

public class ResultTypeBase<T>
{
    public T Value { get; }

    protected ResultTypeBase(T value)
    {
        Value = value;
    }
}

public class Error<TErr> : ResultTypeBase<TErr>
{
    private Error(TErr value) : base(value)
    {
    }

    public static Error<TErr> Build(TErr error)
    {
        return new Error<TErr>(error);
    }
}

public class Success<TOk> : ResultTypeBase<TOk>
{
    private Success(TOk value) : base(value)
    {
    }

    public static Success<TOk> Build(TOk ok)
    {
        return new Success<TOk>(ok);
    }
    
}

public class Result<TOk, TErr>
{
    public object? Value { get; private set; }
    protected Result(){}

    public static Result<TOk, TErr> Success(TOk success)
    {
        return new Result<TOk, TErr>() {Value = Success<TOk>.Build(success)};
    }

    public static Result<TOk, TErr> Error(TErr error)
    {
        return new Result<TOk, TErr>(){Value = Error<TErr>.Build(error)};
    }
}