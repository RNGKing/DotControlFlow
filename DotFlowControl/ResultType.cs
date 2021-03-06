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

    private Action<TOk> _ok;
    private Action<TErr> _err;
    
    #endregion
    
    #region Private and Protected Ctor

    private Result(){}
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
        if (_ok is null && _err is null)
            throw new MatchException("To match, at least one of the functions must be defined");
        switch (Value)
        {
            case Success<TOk> success:
                _ok?.Invoke(success.Value);
                break;
            case Error<TErr> error:
                _err?.Invoke(error.Value);
                break;
        }
    }
    #endregion
    public Result<TOk, TErr> Success(Action<TOk> success)
    {
        _ok = success;
        return this;
    }

    public Result<TOk, TErr> Error(Action<TErr> error)
    {
        _err = error;
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