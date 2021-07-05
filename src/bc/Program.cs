using Biza.CodeAnalysis;
using Biza.CodeAnalysis.Binding;
using Biza.CodeAnalysis.Syntaxt;
using System;
using System.Linq;
using static System.Console;

namespace Biza
{
    class Program
    {
        static void Main(string[] args)
        {
            var showTree = true;

            while (true)
            {
                Write("> ");
                var line = ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#showTree")
                {
                    showTree = true;
                    WriteLine(showTree ? "Showing parse tree" : "Not showing parse tree");
                    continue;
                }
                else if (line == "cls")
                {
                    Clear();
                    continue;
                }

                var syntaxTree = SyntaxtTree.Parse(line);
                var binder = new Binder();
                var boundExpression = binder.BindExpression(syntaxTree.Root);

                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!diagnostics.Any())
                {
                    var evaluator = new Evaluator(boundExpression);
                    var result = evaluator.Evaluate();
                    WriteLine(result);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in diagnostics)
                        WriteLine(diagnostic);

                    Console.ResetColor();
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Write(indent);
            Write(marker);
            Write(node.Kind);

            if (node is SyntaxToken t && t.Value is not null)
            {
                Write(" ");
                Write(t.Value);
            }

            WriteLine();

            indent += isLast ? "    " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild);
        }
    }
}
