using System.Collections.Generic;

namespace Biza.CodeAnalysis.Syntaxt
{
    internal sealed class Lexer
    {
        private int _position;
        private readonly string _text;

        private List<string> _diagnostics = new List<string>();

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;
        
        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';

                return _text[_position];
            }
        }

        private void Next() => _position++;

        public SyntaxToken Lex()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                var start = _position;

                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);

                if (!int.TryParse(text, out var value))
                    _diagnostics.Add($"The number {_text} isn't valid Int32");

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                var start = _position;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);

                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            if (char.IsLetter(Current))
            {
                var start = _position;

                while (char.IsLetter(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);

                return new SyntaxToken(kind, start, text, null);
            }

            var token = Current switch
            {
                '+' => new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null),
                '-' => new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null),
                '*' => new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null),
                '/' => new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null),
                '(' => new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null),
                ')' => new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null),
                _ => null
            };

            if (token is not null)
                return token;

            _diagnostics.Add($"ERROR: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}