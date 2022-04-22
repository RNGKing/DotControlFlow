namespace DotFlowControl;

public class ResultTypeBase<T>
{
    public T Value { get; }

    protected ResultTypeBase(T value)
    {
        Value = value;
    }
}

public sealed class Error<TErr> : ResultTypeBase<TErr>
{
    private Error(TErr value) : base(value)
    {
    }

    public static Error<TErr> Build(TErr error)
    {
        return new Error<TErr>(error);
    }
}

public sealed class Success<TOk> : ResultTypeBase<TOk>
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
    #region Success Value
    public object? Value { get; private set; }
    #endregion
    
    #region Success and Error Evaluators

    private Action<TOk> Ok = null;
    private Action<TErr> Err = null;
    
    #endregion
    
    #region Private and Protected Ctor
    protected Result(){}
    #endregion
    
    #region Success and Error Builders
    public static Result<TOk, TErr> Success(TOk success)
    {
        return new Result<TOk, TErr>() {Value = Success<TOk>.Build(success)};
    }

    public static Result<TOk, TErr> Error(TErr error)
    {
        return new Result<TOk, TErr>(){Value = Error<TErr>.Build(error)};
    }
    #endregion
    
    #region Evaluators

    #region Private Evaluators
    private void Match()
    {
        if (Ok is null && Err is null)
            throw new MatchException("To match, at least one of the functions must be defined");
        switch (Value)
        {
            case Success<TOk> success:
                Ok?.Invoke(success.Value);
                break;
            case Error<TErr> error:
                Err?.Invoke(error.Value);
                break;
        }
    }
    #endregion
    public Result<TOk, TErr> Success(Action<TOk> success)
    {
        Ok = success;
        return this;
    }

    public Result<TOk, TErr> Error(Action<TErr> error)
    {
        Err = error;
        return this;
    }

    public void Match(Action<TOk> success, Action<TErr> error)
    {
        this.Success(success).Error(error).Match();
    }

    public void Match(Action<TErr> error)
    {
        this.Error(error).Match();
    }

    public void Match(Action<TOk> success)
    {
        this.Success(success).Match();
    }
    
    #endregion
}

public class MatchException : Exception
{
    public MatchException(string msg) : base(msg)
    {
    }

    public MatchException() : base()
    {
    }
}