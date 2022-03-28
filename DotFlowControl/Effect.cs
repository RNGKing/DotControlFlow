namespace DotFlowControl;

public class Effect<TErr>
{
    public object? Value { get; private set; }
    
    private Effect(){}

    #region Public Builders
    public static Effect<TErr> Error(TErr error)
    {
        return new Effect<TErr>()
        {
            Value = Error<TErr>.Build(error)
        };
    }
    public static Effect<TErr> Success()
    {
        return new Effect<TErr>()
        {
            Value = new object()
        };
    }
    #endregion
}