using System.Collections.Generic;
using System.Linq;

namespace Biza.CodeAnalysis
{
    public sealed class SyntaxtTree
    {
        public SyntaxtTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxtTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}