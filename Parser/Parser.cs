using System;
using Rook.Tree;
using Rook.Tokenizer;
using System.Collections.Generic;

namespace Rook.Parse {
    public class Parser {

        private Lexer lexer;
        private List<AST> trees = new List<AST>();
        public Parser(InputStream stream) {
            this.lexer = new Lexer(stream);
        }
        public Parser(Lexer lexer) {
            this.lexer = lexer;
        }
        public Parser(string code) {
            this.lexer = new Lexer(code);
        }
        public List<AST> Parse() {
            this.trees = this.GetTrees();
            return this.trees;
        }
        public List<AST> GetTrees() {
            List <AST> innerTrees = new List<AST>();
            Token token = new Token(TokenType.IDENTIFIER, "begin", 1, 1);
            while(token.Type != TokenType.EOF) 
            {
                token = lexer.Next();
                switch(token.Type) 
                {
                    //Keywords like LET, IF, WHILE
                    case TokenType.KEYWORD:
                        if (token.Value == KeyWords.LET) 
                        {
                            this.ParseAssignment(token, ref innerTrees);
                        } else if (token.Value == KeyWords.IF) {
                            innerTrees.Add(this.ParseIf(token));
                        }
                        break;
                    //When accessing a variable, 
                    //when performing an arithmetic operation, 
                    //or when calling a function
                    case TokenType.IDENTIFIER:
                        if (lexer.Peek().Type == TokenType.SEMI_COLON) 
                        {
                            innerTrees.Add(new Variable(token.Value));
                        } 
                        else if (lexer.Peek().Type == TokenType.IS) 
                        //Reassigning a variable
                        {
                            lexer.Previous();
                            this.ParseReAssignment(token, ref innerTrees);
                        }
                        break;
                    //
                }
                if(token.Type == TokenType.SEMI_COLON) {
                    continue;
                }
            }
            return innerTrees;
        }
        public void ParseAssignment(Token token, ref List<AST> innerTrees) {
            Token identifier = this.Fetch(TokenType.IDENTIFIER);
            AST right;
            if(lexer.Peek().Type == TokenType.SEMI_COLON) {
                right = new Tree.Null();
            } else {
                this.Consume(TokenType.IS);
                right = this.Assignee();
            }
            innerTrees.Add(new Assignment(identifier.Value, right));
        }
        public AST ParseIf(Token token) {
            AST condition = this.Expression(lexer.Next());
            this.Consume(TokenType.THEN);
            AST thenBlock = this.Expression(lexer.Next());
            AST elseBlock = null;
            if(lexer.Peek().Is(TokenType.ELSE)) {
                this.Consume(TokenType.ELSE);
                elseBlock = this.Expression(lexer.Next());
            }
            return new IfStatement(condition, thenBlock, elseBlock);
        }
        public void ParseReAssignment(Token token, ref List<AST> innerTrees) {
            Token identifier = this.Fetch(TokenType.IDENTIFIER);
            this.Consume(TokenType.IS);
            AST right = this.Assignee();
            innerTrees.Add(new ReAssignment(identifier.Value, right));
        }
        public AST Assignee() {
            Token token = lexer.Next();
            switch(token.Type) {
                case TokenType.NUMBER:
                    if(lexer.Peek().Is(TokenType.SEMI_COLON)) {
                        return new Number(token.Value);
                    } else if(lexer.Peek().isOperator()) {
                        return this.Expression(token);
                    } else {
                        this.NotImplemented(lexer.Peek());
                    }
                    break;
                case TokenType.IDENTIFIER: 
                    if(lexer.Peek().Is(TokenType.SEMI_COLON)) {
                        return new Variable(token.Value);
                    } else if(lexer.Peek().isOperator()) {
                        return this.Expression(token);
                    } else {
                        this.NotImplemented(lexer.Peek());
                    }
                    break;
                case TokenType.BOOLEAN:
                    if(lexer.Peek().Is(TokenType.SEMI_COLON)) {
                        return new Tree.Boolean(token.Value);
                    } else if(lexer.Peek().isOperator()) {
                        return this.Expression(token);
                    } else {
                        this.NotImplemented(lexer.Peek());
                    }
                    break;
                case TokenType.STRING:
                    if(lexer.Peek().Is(TokenType.SEMI_COLON)) {
                        return new Tree.String(token.Value);
                    } else {
                        this.NotImplemented(lexer.Peek());
                    }
                    break;
                case TokenType.NULL:
                    if(lexer.Peek().Is(TokenType.SEMI_COLON)) {
                        return new Tree.Null();
                    } else {
                        this.NotImplemented(lexer.Peek());
                    }
                    break;
                case TokenType.LEFT_BRACKET:
                    return this.Expression(token);
                case TokenType.IF:
                    return this.ParseIf(token);
                default:
                    throw new Exception();

            }
            throw new Exception();
        }
        public AST simpleTree(Token token)
        {
            switch(token.Type)
            {
                case TokenType.IDENTIFIER:
                    if(lexer.Peek().Is(TokenType.LEFT_BRACKET))
                    {
                        //Should be a function tree
                        this.NotImplemented(lexer.Peek());
                    } else {
                        return new Variable(token.Value);
                    }
                    break;
                case TokenType.NUMBER:
                    return new Number(token.Value);
                case TokenType.STRING:
                    return new Tree.String(token.Value);
                case TokenType.BOOLEAN:
                    if(token.Value.Equals(KeyWords.TRUE)) {
                        return new Tree.Boolean(KeyWords.TRUE);
                    } else if (token.Value.Equals(KeyWords.FALSE)) {
                        return new Tree.Boolean(KeyWords.FALSE);
                    }
                    break;
                default:
                    this.Error("Not supported.", token);
                    break;
            }
            throw new Exception();
        }
        public AST Expression(Token token)
        {
            List<dynamic> list = new List<dynamic>();
            Token current = token;
            while(!current.Is(TokenType.EOF))
            {
                if(current.isOperator()) 
                {
                    list.Add(current);
                    current = lexer.Next();
                    continue;
                } else if (current.Is(TokenType.SEMI_COLON)) {
                    break;
                } else if (current.Is(TokenType.THEN)) {
                    //This is for if statements and while statements
                    lexer.Previous();
                    break;
                } else if (current.Is(TokenType.ELSE)) {
                    //This is for if statements and while statements
                    lexer.Previous();
                    break;
                } else {
                    list.Add(this.simpleTree(current));
                    current = lexer.Next();
                    continue;
                }
            }
            return new Expression(list);
        }
        /*
        * Consumes an expected token otherwise throws an Error.
        * @param TokenType type
        * @param string error
        */
        public void Consume(TokenType type) {
            Token token = lexer.Next();
            if(token.Type != type) {
                throw new Exception("Unexpected token (" + token.Column + "," + token.Row + ")");
            }
        }
        public Token Fetch(TokenType type) {
            Token token = lexer.Next();
            if(token.Type != type) {
                throw new Exception(type.ToString() + " expected (" + token.Column + "," + token.Row + ")");
            }
            return token;
        }
        public bool TokenIs(Token token, TokenType type)
        {
            return token.Type == type;
        }
        public void Error(string msg, Token token) {
            throw new Exception(msg + " (" + token.Column + "," + token.Row + ")");
        }

        public void NotImplemented(Token token) {
            throw new NotImplementedException("[NOT IMPLEMENTED] (" + token.Column + "," + token.Row + ")");
        }
    }
}