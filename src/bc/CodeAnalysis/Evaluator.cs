using Biza.CodeAnalysis.Binding;
using System;

namespace Biza.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(BoundExpression node)
        {
            return node switch
            {
                BoundLiteralExpression n => (int)n.Value,
                BoundUnaryExpression u => EvaluateUnaryExpression(u),
                BoundBinaryExpression b => EvaluateBinaryExpression(b),
                _ => throw new Exception($"Unexpected node {node.Kind}")
            };
        }

        private int EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

            return u.OperatorKind switch
            {
                BoundUnaryOperatorKind.Identity => operand,
                BoundUnaryOperatorKind.Negation => -operand,
                _ => throw new Exception($"Unexpected unary operator {u.OperatorKind}")
            };
        }

        private int EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            return b.OperatorKind switch
            {

                BoundBinaryOperatorKind.Addition => left + right,
                BoundBinaryOperatorKind.Subtraction => left - right,
                BoundBinaryOperatorKind.Multiplication=> left * right,
                BoundBinaryOperatorKind.Division => left / right,
                _ => throw new Exception($"Unexpected binary operator {b.OperatorKind}")
            };
        }
    }
}