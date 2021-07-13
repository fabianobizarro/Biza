using System;

namespace Biza.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(VariableSymbol variable, BoundExpression expression)
        {
            Varible = variable;
            Expression = expression;
        }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override Type Type => Expression.Type;
        public VariableSymbol Varible { get; }
        public BoundExpression Expression { get; }
    }
}
