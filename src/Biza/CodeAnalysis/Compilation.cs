using Biza.CodeAnalysis.Binding;
using Biza.CodeAnalysis.Syntaxt;
using System;
using System.Linq;

namespace Biza.CodeAnalysis
{
    public sealed class Compilation
    {
        public Compilation(SyntaxtTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public SyntaxtTree SyntaxTree { get; }

        public EvaluationResult Evaluate()
        {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(SyntaxTree.Root);

            var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics);
            if (!diagnostics.Any())
            {
                return new EvaluationResult(diagnostics, null);
            }

            var evaluator = new Evaluator(boundExpression);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<string>(), value);
        }
    }
}
