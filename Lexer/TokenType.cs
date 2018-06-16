using System;

namespace Rook.Tokenizer {
    [Flags]
    public enum TokenType {
        PLUS,
        MINUS,
        DIVIDE,
        TIMES,
        MODULUS,
        LESS_THAN,
        LESS_THAN_OR_EQUAL_TO,
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL_TO,
        EQUAL_TO,
        NOT_EQUAL_TO,
        LOGICAL_AND,
        LOGICAL_OR,
        SEMI_COLON,
        COLON,
        IS,
        KEYWORD,
        IDENTIFIER,
        NUMBER,
        BOOLEAN,
        NULL,
        STRING,
        LEFT_BRACKET,
        RIGHT_BRACKET,
        RIGHT_CURLY_BRACKET,
        LEFT_CURLY_BRACKET,
        FULL_STOP,
        IF,
        THEN,
        COMMA,
        ELSE,
        EOF,
   }
}