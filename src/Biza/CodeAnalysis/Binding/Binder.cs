using Biza.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Biza.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private readonly Dictionary<VariableSymbol, object> _variables;
        private readonly DiagnosticBag _diagnostics = new();

        public Binder(Dictionary<VariableSymbol, object> variables)
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
            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (variable is null)
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }

            return new BoundVariableExpression(variable);
        }
        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);

            var existingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            if (existingVariable is not null)
                _variables.Remove(existingVariable);

            var variable = new VariableSymbol(name, boundExpression.Type);
            _variables[variable] = null;

            return new BoundAssignmentExpression(variable, boundExpression);
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
