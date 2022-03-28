using System.Collections;

namespace DotFlowControl.Operators;

public abstract class OperatorData{}

public abstract class OperatorInfo<TErr>
{
    public Func<OperatorData, Effect<TErr>> Operator { get; }
    public OperatorData Data { get; }
    public OperatorInfo(Func<OperatorData, Effect<TErr>> operation, OperatorData data)
    {
        Operator = operation;
        Data = data;
    }
}

public static class SequenceOperators<TErr>
{
    public static Effect<TErr> RunSequence(IEnumerable<OperatorInfo<TErr>> operations)
    {
        foreach (var operation in operations)
        {
            var result = operation.Operator(operation.Data);
            if (result.Value is Error<TErr> error)
                return result;
        }
        return Effect<TErr>.Success();
    }
}