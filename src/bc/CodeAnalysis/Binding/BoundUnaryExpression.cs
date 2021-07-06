using System;

namespace Biza.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator operatorKind, BoundExpression operand)
        {
            Op = operatorKind;
            Operand = operand;
        }

        public override Type Type => Operand.Type;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }
    }
}
