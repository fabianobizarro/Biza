using System.Collections.Generic;

namespace Biza.CodeAnalysis
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind {get;}

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}