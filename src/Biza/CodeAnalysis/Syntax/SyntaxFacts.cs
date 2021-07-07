namespace Biza.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind) => kind switch
        {
            SyntaxKind.PlusToken or SyntaxKind.MinusToken or SyntaxKind.BangToken => 6,
            _ => 0
        };

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind) => kind switch
        {
            SyntaxKind.StarToken or SyntaxKind.SlashToken => 5,
            SyntaxKind.PlusToken or SyntaxKind.MinusToken => 4,
            SyntaxKind.EqualsEqualsToken or SyntaxKind.BangEqualsToken => 3,
            SyntaxKind.AmpersandAmpersandToken or SyntaxKind.AndKeyword=> 2,
            SyntaxKind.PipePipeToken or SyntaxKind.OrKeyword => 1,
            _ => 0
        };

        internal static SyntaxKind GetKeywordKind(string text) => text switch
        {
            "true" => SyntaxKind.TrueKeyword,
            "false" => SyntaxKind.FalseKeyword,
            "and" => SyntaxKind.AndKeyword,
            "or" => SyntaxKind.OrKeyword,
            _ => SyntaxKind.IdentifierToken,
        };
    }
}
