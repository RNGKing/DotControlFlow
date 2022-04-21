namespace DotFlowControl;

public class Effect<TErr>
{
    #region Public Property
    
    public object? Value { get; private set; }
    
    #endregion
    
    #region Private Ctor
    
    private Effect(){}

    #endregion
    
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