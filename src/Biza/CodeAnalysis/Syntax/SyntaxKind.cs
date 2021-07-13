namespace Biza.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        SlashToken,
        StarToken,
        BangToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken,
        CloseParenthesisToken,
        OpenParenthesisToken,
        IdentifierToken,

        // Keywords
        FalseKeyword,
        TrueKeyword,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        OrKeyword,
        AndKeyword,
        EqualsToken,
        AssignmentExpression,
        NameExpression,
    }
}