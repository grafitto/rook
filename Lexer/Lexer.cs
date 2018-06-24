using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace Rook.Tokenizer {
    public class Lexer {

        private int position;
        public string FileName { get { return this.stream.FileName; } }
        InputStream stream;
        List<Token> tokens;
        public Lexer(string fileName) {
            try{
                string code = File.ReadAllText(fileName);
                stream = new InputStream(code, fileName);
            } catch(IOException e) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
            this.tokens = new List<Token>();
            this.position = 0;
            this.Tokenize();
        }
        public Lexer(InputStream stream) {
            this.stream = stream;
            this.tokens = new List<Token>();
            this.position = 0;
            this.Tokenize();
        }
        public void Tokenize() {
            while(!this.stream.End()) {
                char character = stream.Next();
                
                if(character.Equals(CharacterTypes.FULL_COLON)) {
                    if(stream.Peek().Equals(CharacterTypes.EQUALS)) {
                        string op = character.ToString() + stream.Next().ToString();
                        tokens.Add(new Token(TokenType.IS, op, this.FileName, this.stream.Column, this.stream.Row));
                    } else {
                        tokens.Add(new Token(TokenType.COLON, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                    }
                } else if (character.Equals(CharacterTypes.COMMA)) {
                    //- (MINUS)
                    tokens.Add(new Token(TokenType.COMMA, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.FULL_STOP)) {
                    //- (MINUS)
                    tokens.Add(new Token(TokenType.FULL_STOP, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.LEFT_SQUARE_BRACKET)) {
                    tokens.Add(new Token(TokenType.LEFT_SQUARE_BRACKET, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.RIGHT_SQUARE_BRACKET)) {
                    tokens.Add(new Token(TokenType.RIGHT_SQUARE_BRACKET, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.SEMI_COLON)) {
                    tokens.Add(new Token(TokenType.SEMI_COLON, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (Regex.IsMatch(character.ToString(), @"[a-zA-Z_]")) {
                    tokens.Add(this.Identifier(character));
                } else if (char.IsDigit(character) || character.Equals('.')) {
                    tokens.Add(this.Number(character));
                } else if (character.Equals(CharacterTypes.DOUBLE_QUOTE)) {
                    tokens.Add(new Token(TokenType.STRING, this.ReadEscaped(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.LEFT_BRACKET)) {
                    //( (LEFT_BRACKET)
                    tokens.Add(new Token(TokenType.LEFT_BRACKET, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.RIGHT_BRACKET)) {
                    //) (RIGHT_BRACKET)
                    tokens.Add(new Token(TokenType.RIGHT_BRACKET, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.LEFT_CURLY_BRACKET)) {
                    //- (LEFT_CURLY_BRACKET)
                    tokens.Add(new Token(TokenType.LEFT_CURLY_BRACKET, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.RIGHT_CURLY_BRACKET)) {
                    //- (LEFT_CURLY_BRACKET)
                    tokens.Add(new Token(TokenType.RIGHT_CURLY_BRACKET, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.PLUS)) {
                    //+ (PLUS)
                    tokens.Add(new Token(TokenType.PLUS, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.MINUS)) {
                    //- (MINUS)
                    tokens.Add(new Token(TokenType.MINUS, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.TIMES)) {
                    //* (TIMES)
                    tokens.Add(new Token(TokenType.TIMES, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.DIVIDE)) {
                    /// (DIVIDE)
                    if(stream.Peek().Equals(CharacterTypes.DIVIDE)) {
                        // This is a comment
                        char next = this.stream.Next();
                        // Read until you heat the new line
                        while(!this.stream.End()) {
                            if(!next.Equals('\n'))
                                next = this.stream.Next();
                            else
                                break;
                        }
                    }else if(stream.Peek().Equals(CharacterTypes.TIMES)) {
                        // /* (MULTILINE COMMENT)
                        char next = this.stream.Next();
                        while(!this.stream.End()) {
                            if(next.Equals(CharacterTypes.TIMES) && this.stream.Peek().Equals(CharacterTypes.DIVIDE)) {
                                break;
                            } else {
                                next = this.stream.Next();
                            }
                        }
                    } else {
                        tokens.Add(new Token(TokenType.DIVIDE, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));   
                    }
                } else if (character.Equals(CharacterTypes.MODULUS)) {
                    // % (MODULUS)
                    tokens.Add(new Token(TokenType.MODULUS, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                } else if (character.Equals(CharacterTypes.LESS_THAN)) {
                    if(stream.Peek().Equals(CharacterTypes.EQUALS)){
                        //<= 
                        string lessOrEqual = character.ToString() + stream.Next().ToString();
                        tokens.Add(new Token(TokenType.LESS_THAN_OR_EQUAL_TO, lessOrEqual, this.FileName, this.stream.Column, this.stream.Row));
                    } else {
                        //<
                        tokens.Add(new Token(TokenType.LESS_THAN, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                
                    }
                } else if (character.Equals(CharacterTypes.GREATER_THAN)) {
                    if(stream.Peek().Equals(CharacterTypes.EQUALS)){
                        //>= 
                        string greaterOrEqual = character.ToString() + stream.Next().ToString();
                        tokens.Add(new Token(TokenType.GREATER_THAN_OR_EQUAL_TO, greaterOrEqual, this.FileName, this.stream.Column, this.stream.Row));
                    } else {
                        //>
                        tokens.Add(new Token(TokenType.GREATER_THAN, character.ToString(), this.FileName, this.stream.Column, this.stream.Row));
                
                    }
                } else if (character.Equals(CharacterTypes.EQUALS)) {
                    if(stream.Peek().Equals(CharacterTypes.EQUALS)){
                        // ==
                        string equality = character.ToString() + stream.Next().ToString();
                        tokens.Add(new Token(TokenType.EQUAL_TO, equality, this.FileName, this.stream.Column, this.stream.Row));
                    } else {
                        //<
                        stream.Error("Character " + character + " not supported.");
                    }
                } else if(character.Equals(CharacterTypes.NOT)) {
                    if(stream.Peek().Equals(CharacterTypes.EQUALS)) {
                        string notEqualTo = character.ToString() + stream.Next().ToString();
                        tokens.Add(new Token(TokenType.NOT_EQUAL_TO, notEqualTo, this.FileName, this.stream.Column, this.stream.Row));
                    } else {
                        stream.Error("Character " + character + " not supported");
                    }
                } else if(character.Equals(CharacterTypes.LOGICAL_AND)) {
                    if(stream.Peek().Equals(CharacterTypes.LOGICAL_AND)) {
                        string and = character.ToString() + stream.Next().ToString();
                        tokens.Add(new Token(TokenType.LOGICAL_AND, and, this.FileName, this.stream.Column, this.stream.Row));
                    } else {
                        stream.Error("Character " + character + " not supported");
                    }
                } else if(character.Equals(CharacterTypes.LOGICAL_OR)) {
                    if(stream.Peek().Equals(CharacterTypes.LOGICAL_OR)) {
                        string or = character.ToString() + stream.Next().ToString();
                        tokens.Add(new Token(TokenType.LOGICAL_OR, or, this.FileName, this.stream.Column, this.stream.Row));
                    } else {
                        stream.Error("Character " + character + " not supported");
                    }
                }
            }
            tokens.Add(new Token(TokenType.EOF, "end", this.FileName, this.stream.Column, this.stream.Row));
        }
        public Token Identifier(char character) {
            if(character.Equals('l') && this.stream.Peek().Equals('e') && this.stream.Peek(1).Equals('t') && char.IsWhiteSpace(this.stream.Peek(2))) {
                this.stream.Next(); this.stream.Next(); this.stream.Next();
                return new Token(TokenType.KEYWORD, KeyWords.LET, this.FileName, this.stream.Column, this.stream.Row);
            } else if(character.Equals('F') && this.stream.Peek().Equals('a') && this.stream.Peek(1).Equals('l') && this.stream.Peek(2).Equals('s') && this.stream.Peek(3).Equals('e')){
                //False
                this.stream.Next(); this.stream.Next(); this.stream.Next(); this.stream.Next();
                return new Token(TokenType.BOOLEAN, KeyWords.FALSE, this.FileName, this.stream.Column, this.stream.Row);
            } else if(character.Equals('T') && this.stream.Peek().Equals('r') && this.stream.Peek(1).Equals('u') && this.stream.Peek(2).Equals('e')) {
                //True
                this.stream.Next(); this.stream.Next(); this.stream.Next();
                return new Token(TokenType.BOOLEAN, KeyWords.TRUE, this.FileName, this.stream.Column, this.stream.Row);
            } else if(character.Equals('N') && this.stream.Peek().Equals('u') && this.stream.Peek(1).Equals('l') && this.stream.Peek(2).Equals('l')) {
                // Null
                this.stream.Next(); this.stream.Next(); this.stream.Next();
                return new Token(TokenType.NULL, KeyWords.NULL, this.FileName, this.stream.Column, this.stream.Row);
            } else if(character.Equals('i') && this.stream.Peek().Equals('f')) {
                // If
                this.stream.Next();
                return new Token(TokenType.IF, KeyWords.IF, this.FileName, this.stream.Column, this.stream.Row);
            } else if(character.Equals('e') && this.stream.Peek().Equals('l') && this.stream.Peek(1).Equals('s') && this.stream.Peek(2).Equals('e')) {
                // Null
                this.stream.Next(); this.stream.Next(); this.stream.Next();
                return new Token(TokenType.ELSE, KeyWords.ELSE, this.FileName, this.stream.Column, this.stream.Row);
            } else if(character.Equals('t') && this.stream.Peek().Equals('h') && this.stream.Peek(1).Equals('e') && this.stream.Peek(2).Equals('n')) {
                // Null
                this.stream.Next(); this.stream.Next(); this.stream.Next();
                return new Token(TokenType.THEN, KeyWords.THEN, this.FileName, this.stream.Column, this.stream.Row);
            } else {
                string identifier = character.ToString();
                char chunk = this.stream.Next();
                while(!this.stream.End()) {
                    if(Regex.IsMatch(chunk.ToString(), @"[a-zA-Z_]")) {
                        identifier += chunk.ToString();
                        chunk = this.stream.Next();
                    } else {
                        break;
                    }
                }
                this.stream.Previous();
                return new Token(TokenType.IDENTIFIER, identifier, this.FileName, this.stream.Column, this.stream.Row);
            }
        }

        private Token Number(char character) {
            string number = character.ToString();
            char chunk = this.stream.Next();

            do {
                if (char.IsDigit(chunk)) {
                    number = number + chunk.ToString();
                    chunk = this.stream.Next();
                }else if(chunk.Equals('.') && char.IsDigit(this.stream.Peek())) {
                    number = number + chunk.ToString();
                    chunk = this.stream.Next();
                } else {
                    break;
                }
            } while (!this.stream.End());
            this.stream.Previous();
            return new Token( TokenType.NUMBER, number, this.FileName, this.stream.Column, this.stream.Row);
        }
        public string ReadEscaped(char end = CharacterTypes.DOUBLE_QUOTE) {
            bool escaped = false;
            string value = "";
            while (!stream.End()) {
                var ch = stream.Next();
                if (escaped) {
                    value += ch.ToString();
                    escaped = false;
                } else if (ch.Equals('\\')) {
                    escaped = true;
                } else if (ch.Equals(end)) {
                    break;
                } else {
                    value += ch.ToString();
                }
            }
            return value;
        }
        /**
        * Fetches the next token in the list
        */
        public Token Next() {
            return tokens[this.position++];
        }
        public Token Peek(int ahead = 0) {
            return tokens[this.position + ahead];
        }
        public void Previous() {
            this.position--;
        }
    }
}