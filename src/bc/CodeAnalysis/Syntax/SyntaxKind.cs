namespace Biza.CodeAnalysis.Syntaxt
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
        CloseParenthesisToken,
        OpenParenthesisToken,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
    }
}