using System;

namespace Biza.CodeAnalysis
{
    public sealed class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            return node switch
            {
                LiteralExpressionSyntax n => (int)n.NumberToken.Value,
                UnaryExpressionSyntax u => EvaluateUnaryExpression(u),
                BinaryExpressionSyntax b => EvaluateBinaryExpression(b),
                ParenthesizedExpressionSyntax p => EvaluateExpression(p.Expression),
                _ => throw new Exception($"Unexpected node {node.Kind}")
            };
        }

        private int EvaluateUnaryExpression(UnaryExpressionSyntax u)
        {
            var operand = EvaluateExpression(u.Operand);

            return u.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => operand,
                SyntaxKind.MinusToken => -operand,
                _ => throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}")
            };
        }

        private int EvaluateBinaryExpression(BinaryExpressionSyntax b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            return b.OperationToken.Kind switch
            {
                SyntaxKind.PlusToken => left + right,
                SyntaxKind.MinusToken => left - right,
                SyntaxKind.StarToken => left * right,
                SyntaxKind.SlashToken => left / right,
                _ => throw new Exception($"Unexpected binary operator {b.OperationToken.Kind}")
            };
        }
    }
}