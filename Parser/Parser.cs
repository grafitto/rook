using System;
using Rook.Tree;
using Rook.Errors;
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
        public Parser(string fileName) {
            this.lexer = new Lexer(fileName);
        }
        public List<AST> Parse() {
            this.trees = this.GetTrees();
            return this.trees;
        }
        public List<AST> GetTrees() {
            List <AST> innerTrees = new List<AST>();
            Token token = new Token(TokenType.IDENTIFIER, "begin", "", 1, 1);
            while(!token.Is(TokenType.EOF)) 
            {
                token = lexer.Next();
                switch(token.Type) 
                {
                    //Keywords like LET, IF, WHILE
                    case TokenType.KEYWORD:
                        if (token.Value == KeyWords.LET) 
                        {
                            innerTrees.Add(this.ParseAssignment(token));
                        } else if (token.Value == KeyWords.IF) {
                            innerTrees.Add(this.ParseIf(token));
                        }
                        break;
                    case TokenType.IF:
                        innerTrees.Add(this.ParseIf(token));
                        break;
                    //When accessing a variable, 
                    //when performing an arithmetic operation, 
                    //or when calling a function
                    case TokenType.IDENTIFIER:
                        if (lexer.Peek().Is(TokenType.SEMI_COLON)) 
                        {
                            innerTrees.Add(new Variable(token.Value));
                        } 
                        else if (lexer.Peek().Is(TokenType.IS)) {
                            //Reassigning a variable
                            lexer.Previous();
                            innerTrees.Add(this.ParseReAssignment(token));
                        } else if(lexer.Peek().Is(TokenType.LEFT_SQUARE_BRACKET)) {
                            innerTrees.Add(this.ParseListReAssignment(token));

                        }else if(lexer.Peek().Is(TokenType.LEFT_BRACKET)) {
                            //Function
                            innerTrees.Add(this.ParseFunctionCall(token));
                        } else if(lexer.Peek().isOperator()) {
                            innerTrees.Add(this.Expression(token));
                        }
                        break;
                    case TokenType.NUMBER:
                        innerTrees.Add(this.Expression(token));
                        break;
                    //
                }
                if(token.Is(TokenType.SEMI_COLON)) {
                    continue;
                }
                if(token.Is(TokenType.RIGHT_CURLY_BRACKET)) {
                    break;
                }
            }
            return innerTrees;
        }
        public AST ParseListReAssignment(Token token) {
            this.Consume(TokenType.LEFT_SQUARE_BRACKET);
            Token index = lexer.Next();
            this.Consume(TokenType.RIGHT_SQUARE_BRACKET);
            AST indexer = null;
            if(index.Is(TokenType.IDENTIFIER)) {
                indexer = new Tree.Variable(index.Value);
            } else if (index.Is(TokenType.NUMBER)) {
                indexer = new Tree.Number(index.Value);
            } else {
                this.Error("Only variables or numbers can be used to access list items", index);
            }
            AST right = null;
            if(lexer.Peek().Is(TokenType.IS)) {
                //there is the right part;
                this.Consume(TokenType.IS);
                right = this.Assignee();
            } else if (lexer.Peek().Is(TokenType.SEMI_COLON)) {
                right = new Tree.Null();
            }
            ListModify modifier = new ListModify(token.Value, indexer, right);
            return new ReAssignment(token.Value, modifier);
        }
        public AST ParseFunctionCall(Token token) {
            Token name = token;
            this.Consume(TokenType.LEFT_BRACKET);
            List<AST> arguments = this.ExtractArguments();
            this.Consume(TokenType.RIGHT_BRACKET);
            return new FunctionCall(name.Value, arguments);
        }
        public List<AST> ExtractArguments() {
            List<AST> arguments = new List<AST>();
            Token current = lexer.Next();
            while(!current.Is(TokenType.EOF)) {      
                if(current.Is(TokenType.RIGHT_BRACKET)) {
                    lexer.Previous();
                    return arguments;
                } else if(lexer.Peek().Is(TokenType.LEFT_SQUARE_BRACKET)) {
                    arguments.Add(this.Expression(current, true));
                
                    current = lexer.Next();
                    continue;
                } else if(lexer.Peek().Is(TokenType.COMMA)) {
                    arguments.Add(this.simpleTree(current));
                    current = lexer.Next();
                    continue;
                } else if (lexer.Peek().Is(TokenType.RIGHT_BRACKET)) {
                    arguments.Add(this.simpleTree(current));
                    return arguments;
                } else if(current.Is(TokenType.COMMA)) {
                    current = lexer.Next();
                    arguments.Add(this.Expression(current, true));
                    current = lexer.Next();
                    continue;
                } else if(lexer.Peek().isOperator()){
                    arguments.Add(this.Expression(current, true));
                    current = lexer.Next();
                    continue;
                } else if(lexer.Peek().Is(TokenType.LEFT_BRACKET)) {
                    arguments.Add(this.ParseFunctionCall(current));
                    current = lexer.Next();
                    continue;
                } else {
                    this.NotImplemented(lexer.Next());
                }
            }
            return arguments;
        }
        public Assignment ParseAssignment(Token token) {
            Token identifier = this.Fetch(TokenType.IDENTIFIER);
            AST right;
            if(lexer.Peek().Type == TokenType.SEMI_COLON) {
                right = new Tree.Null();
            } else {
                this.Consume(TokenType.IS);
                right = this.Assignee();
            }
            return new Assignment(identifier.Value, right);
        }
        public AST ParseIf(Token token) {
            AST condition = this.Expression(lexer.Next());
            this.Consume(TokenType.THEN);
            AST thenBlock = null;
            Token next = lexer.Next();
            if(lexer.Peek().Is(TokenType.LEFT_BRACKET)) {
                thenBlock = this.ParseFunctionCall(next);
            }else if(next.Is(TokenType.LEFT_CURLY_BRACKET)) {
                thenBlock = this.FunctionDefinition(next);
            } else {
                thenBlock = this.Expression(next);
            }
            AST elseBlock = null;
            if(lexer.Peek().Is(TokenType.ELSE)) {
                this.Consume(TokenType.ELSE);
                Token nextElse = lexer.Next();
                if(lexer.Peek().Is(TokenType.LEFT_BRACKET)) {
                    elseBlock = this.ParseFunctionCall(nextElse);
                } else if (next.Is(TokenType.LEFT_CURLY_BRACKET)) {
                    elseBlock = this.FunctionDefinition(nextElse);
                } else {
                    elseBlock = this.Expression(nextElse);
                }
            }
            return new IfStatement(condition, thenBlock, elseBlock);
        }
        public ReAssignment ParseReAssignment(Token token) {
            Token identifier = this.Fetch(TokenType.IDENTIFIER);
            this.Consume(TokenType.IS);
            AST right = this.Assignee();
            return new ReAssignment(identifier.Value, right);
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
                    } else if(lexer.Peek().isOperator() || lexer.Peek().Is(TokenType.LEFT_SQUARE_BRACKET)) {
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
                    } else if(lexer.Peek().isOperator()) {
                        return this.Expression(token);
                    }else {
                        this.NotImplemented(lexer.Peek());
                    }
                    break;
                case TokenType.NULL:
                    if(lexer.Peek().Is(TokenType.SEMI_COLON)) {
                        return new Tree.Null();
                    } else {
                        this.Error("Unexpected token. ", token);
                    }
                    break;
                case TokenType.LEFT_BRACKET:
                    return this.Expression(token);
                case TokenType.IF:
                    return this.ParseIf(token);
                case TokenType.LEFT_CURLY_BRACKET:
                    return this.FunctionDefinition(token);
                case TokenType.LEFT_SQUARE_BRACKET:
                    return this.ListDefinition(token);
                default:
                    this.Error("Unknown character.", token);
                    break;
            }
            this.Error("Unknown character. ", token);
            throw new SyntaxError();
        }
        public List ListDefinition(Token token) {
            List<AST> items = new List<AST>();
            Token current = token;
            current = lexer.Next();
            while(!current.Is(TokenType.EOF)) {
                if(current.Is(TokenType.COMMA)) {
                    current = lexer.Next();
                    continue;
                } else if(current.Is(TokenType.LEFT_SQUARE_BRACKET)) {
                    items.Add(ListDefinition(current));
                    current = lexer.Next();
                    continue;
                } else if(current.Is(TokenType.RIGHT_SQUARE_BRACKET)) {
                    break;
                } else if(lexer.Peek().Is(TokenType.RIGHT_SQUARE_BRACKET)) {
                    items.Add(this.simpleTree(current));
                    break;
                }
                items.Add(this.Expression(current));
                current = lexer.Next();
            }
            return new Tree.List(items);
        }
        public Function FunctionDefinition(Token token) {
            Token current = token;
            List<string> parameters = new List<string>();
            List<AST> bodyStatements = new List<AST>();
            if(lexer.Peek().Is(TokenType.COLON)) {
                this.Consume(TokenType.COLON);
                parameters = this.ParseFunctionParameters();
            }
            bodyStatements = this.GetTrees();
            return new Function(parameters, bodyStatements);
        }
        public List<string> ParseFunctionParameters() {
            List<string> parameters = new List<string>();
            Token current = this.Fetch(TokenType.LEFT_BRACKET);
            current = lexer.Next();
            while(!current.Is(TokenType.EOF)) {
                if(current.Is(TokenType.COMMA)) {
                    current = lexer.Next();
                    continue;
                } else if(current.Is(TokenType.RIGHT_BRACKET)) {
                    break;
                } 
                parameters.Add(current.Value);
                current = lexer.Next();
            }
            return parameters;
        }
        public ListAccess ParseListAccess(Token token) {
            Token identifier = token;
            this.Consume(TokenType.LEFT_SQUARE_BRACKET);
            Token indexer = lexer.Next();
            AST index = null;
            if(indexer.Is(TokenType.IDENTIFIER)) {
                index = new Tree.Variable(indexer.Value);
            } else if(indexer.Is(TokenType.NUMBER)) {
                index = new Tree.Number(indexer.Value);
            } else if(lexer.Peek().isOperator()) {
                index = this.Expression(indexer);
            } else {
                this.Error("List accesser not allowed. ", indexer);
            }
            this.Consume(TokenType.RIGHT_SQUARE_BRACKET);
            return new ListAccess(identifier.Value, index);
        }
        public AST simpleTree(Token token)
        {
            switch(token.Type)
            {
                case TokenType.IDENTIFIER:
                    if(lexer.Peek().Is(TokenType.LEFT_BRACKET)) {
                        //Should be a function call
                        return this.ParseFunctionCall(token);
                    } else if(lexer.Peek().Is(TokenType.LEFT_SQUARE_BRACKET)){
                        return this.ParseListAccess(token);
                    } else {
                        return new Variable(token.Value);
                    }
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
                case TokenType.NULL:
                    return new Tree.Null();
                default:
                    this.Error("Not supported.", token);
                    break;
            }
            this.Error("Unknown token.", token);
            throw new SyntaxError();
        }
        public AST Expression(Token token, bool fromCall = false)
        {
            List<dynamic> list = new List<dynamic>();
            Token current = token;
            while(!current.Is(TokenType.EOF))
            {
                if(fromCall && current.Is(TokenType.RIGHT_BRACKET)){
                    lexer.Previous();
                    break;
                }else if(current.isOperator()) 
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
                } else if (current.Is(TokenType.COMMA)) {
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
                this.Error("Unexpected token", token);
            }
        }
        public Token Fetch(TokenType type) {
            Token token = lexer.Next();
            if(token.Type != type) {
                this.Error(type.ToString() + " expected", token);
            }
            return token;
        }
        public bool TokenIs(Token token, TokenType type)
        {
            return token.Type == type;
        }
        public void Error(string msg, Token token) {
            throw new SyntaxError("'" + msg + "' in '" + token.FileName + "' [ln " + token.Row + ", Col " + token.Column + "]");
        }

        public void NotImplemented(Token token) {
            throw new NotImplementedException("'[NOT IMPLEMENTED]' in '" + token.FileName +"' [ln " + token.Row + ",Col " + token.Column + "]");
        }
    }
}