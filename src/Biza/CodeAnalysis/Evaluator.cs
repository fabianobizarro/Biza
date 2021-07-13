using Biza.CodeAnalysis.Binding;
using System;
using System.Collections.Generic;

namespace Biza.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;
        private readonly Dictionary<string, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<string, object> variables)
        {
            _root = root;
            _variables = variables;
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
                BoundVariableExpression v => _variables[v.Name],
                BoundAssignmentExpression a => EvaluateAssignmentExpression(a),
                BoundUnaryExpression u => EvaluateUnaryExpression(u),
                BoundBinaryExpression b => EvaluateBinaryExpression(b),
                _ => throw new Exception($"Unexpected node {node.Kind}")
            };
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression a)
        {
            var value = EvaluateExpression(a.Expression);
            _variables[a.Name] = value;
            return value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

            return u.Op.Kind switch
            {
                BoundUnaryOperatorKind.Identity => (int)operand,
                BoundUnaryOperatorKind.Negation => -(int)operand,
                BoundUnaryOperatorKind.LogicalNegation => !(bool)operand,
                _ => throw new Exception($"Unexpected unary operator {u.Op}")
            };
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            return b.Op.Kind switch
            {
                BoundBinaryOperatorKind.Addition => (int)left + (int)right,
                BoundBinaryOperatorKind.Subtraction => (int)left - (int)right,
                BoundBinaryOperatorKind.Multiplication => (int)left * (int)right,
                BoundBinaryOperatorKind.Division => (int)left / (int)right,
                BoundBinaryOperatorKind.LogicalAnd => (bool)left && (bool)right,
                BoundBinaryOperatorKind.LogicalOr => (bool)left || (bool)right,
                BoundBinaryOperatorKind.Equals => Equals(left, right),
                BoundBinaryOperatorKind.NotEquals => !Equals(left, right),

                _ => throw new Exception($"Unexpected binary operator {b.Op}")
            };
        }
    }
}