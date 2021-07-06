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

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            return node switch
            {
                BoundLiteralExpression n => n.Value,
                BoundUnaryExpression u => EvaluateUnaryExpression(u),
                BoundBinaryExpression b => EvaluateBinaryExpression(b),
                _ => throw new Exception($"Unexpected node {node.Kind}")
            };
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = (int)EvaluateExpression(u.Operand);

            return u.OperatorKind switch
            {
                BoundUnaryOperatorKind.Identity => operand,
                BoundUnaryOperatorKind.Negation => -operand,
                _ => throw new Exception($"Unexpected unary operator {u.OperatorKind}")
            };
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            var left = (int)EvaluateExpression(b.Left);
            var right = (int)EvaluateExpression(b.Right);

            return b.OperatorKind switch
            {
                BoundBinaryOperatorKind.Addition => left + right,
                BoundBinaryOperatorKind.Subtraction => left - right,
                BoundBinaryOperatorKind.Multiplication => left * right,
                BoundBinaryOperatorKind.Division => left / right,
                _ => throw new Exception($"Unexpected binary operator {b.OperatorKind}")
            };
        }
    }
}