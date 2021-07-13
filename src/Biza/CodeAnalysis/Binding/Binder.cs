using Biza.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;

namespace Biza.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private readonly Dictionary<string, object> _variables;
        private readonly DiagnosticBag _diagnostics = new();

        public Binder(Dictionary<string, object> variables)
        {
            _variables = variables;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax) => syntax switch
        {
            ParenthesizedExpressionSyntax p => BindParenthesizedExpression(p),

            LiteralExpressionSyntax literal => BindLiteralExpression(literal),
            NameExpressionSyntax name => BindNameExpression(name),
            AssignmentExpressionSyntax assign => BindAssignmentExpression(assign),
            UnaryExpressionSyntax unary => BindUnaryExpression(unary),
            BinaryExpressionSyntax binary => BindBinaryExpression(binary),
            _ => throw new Exception($"Unexpected syntax {syntax.Kind}")
        };

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private static BoundExpression BindLiteralExpression(LiteralExpressionSyntax literal)
        {
            var value = literal.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax) 
        {
            var name = syntax.IdentifierToken.Text;
            if (!_variables.TryGetValue(name, out var value))
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }

            var type = value.GetType();
            return new BoundVariableExpression(name, type);
        }
        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);

            var defaultValue = boundExpression.Type == typeof(int)
                ? (object)0
                : boundExpression.Type == typeof(bool)
                    ? (object)false
                    : null;

            if (defaultValue is null)
                throw new Exception($"Unsupported variable type: {boundExpression.Type}");

            _variables[name] = defaultValue;
            return new BoundAssignmentExpression(name, boundExpression);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator is null)
            {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperator.Type);
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperationToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator is null)
            {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperationToken.Span, syntax.OperationToken.Text, boundLeft.Type, boundRight.Type);
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}
