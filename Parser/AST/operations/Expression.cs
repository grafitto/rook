using System.Collections.Generic;
using Rook.Tokenizer;
using System;

namespace Rook.Tree
{
    public class Expression : AST
    {
        List<dynamic> list;
        Env.Environment env;
        Dictionary<TokenType,int> PRECEDENCE = new Dictionary<TokenType, int>();
        public Expression(List<dynamic> list) : base(TreeType.EXPRESSION)
        {
            this.list = list;
            this.PRECEDENCE.Add(TokenType.LEFT_BRACKET, 19);
            this.PRECEDENCE.Add(TokenType.RIGHT_BRACKET, 19);
            this.PRECEDENCE.Add(TokenType.TIMES, 14);
            this.PRECEDENCE.Add(TokenType.DIVIDE, 14);
            this.PRECEDENCE.Add(TokenType.MODULUS, 14);
            this.PRECEDENCE.Add(TokenType.PLUS, 13);
            this.PRECEDENCE.Add(TokenType.MINUS, 13);
            this.PRECEDENCE.Add(TokenType.LESS_THAN, 11);
            this.PRECEDENCE.Add(TokenType.LESS_THAN_OR_EQUAL_TO, 11);
            this.PRECEDENCE.Add(TokenType.GREATER_THAN, 11);
            this.PRECEDENCE.Add(TokenType.GREATER_THAN_OR_EQUAL_TO, 11);
            this.PRECEDENCE.Add(TokenType.EQUAL_TO, 10);
            this.PRECEDENCE.Add(TokenType.NOT_EQUAL_TO, 10);
            this.PRECEDENCE.Add(TokenType.LOGICAL_AND, 6);
            this.PRECEDENCE.Add(TokenType.LOGICAL_OR, 5);
        }
        public override dynamic Evaluate(Env.Environment env)
        {
            this.env = env;
            return Solve(CreatePostFix());
        }
        public List<dynamic> CreatePostFix()
        {
            List<dynamic> postfix = new List<dynamic>();
            Stack<Token> operatorStack = new Stack<Token>();
            foreach(dynamic item in this.list)
            {
                try{
                    Token operata = item;
                    if (operata.Is(TokenType.LEFT_BRACKET)) {
                        operatorStack.Push(operata);
                    } else if(operata.Is(TokenType.RIGHT_BRACKET)) {
                        while(!operatorStack.Peek().Is(TokenType.LEFT_BRACKET)) {
                            postfix.Add(operatorStack.Pop());
                        }
                        operatorStack.Pop();
                    } else {
                        if(operatorStack.Count > 0){
                            for(int i = 0; i <= operatorStack.Count; i++){
                                if(PRECEDENCE[operatorStack.Peek().Type] >= PRECEDENCE[operata.Type] && !operatorStack.Peek().Is(TokenType.LEFT_BRACKET)) {
                                    postfix.Add(operatorStack.Pop());
                                } else {
                                    break;
                                }
                            }
                        }
                        operatorStack.Push(operata);
                    }
                } catch(Exception){
                    AST operand = item;
                    postfix.Add(operand);
                }
            }
            Token[] remunants = operatorStack.ToArray();
            foreach(Token remunant in remunants)
                postfix.Add(remunant);
                
            return postfix;
        }
        public dynamic Solve(List<dynamic> postfix)
        {
            Stack<dynamic> stack = new Stack<dynamic>();
            foreach(dynamic item in postfix)
            {
                try {
                    Token operata = item;
                    AST op2 = stack.Pop();
                    AST op1 = stack.Pop();
                    stack.Push(this.Apply(operata, op1, op2)); 
                } catch(Exception) {
                    AST oparand = item;
                    stack.Push(oparand);
                }
            }

            return stack.Pop().Evaluate(env);
            /* try{
                return result.Value;
            } catch(Exception) {
                return result.value;
            } */
        }
        public AST Apply(Token operata, AST op1, AST op2)
        {

            switch(operata.Type)
            {
                case TokenType.TIMES:
                    return new Tree.Number((op1.Evaluate(this.env) * op2.Evaluate(this.env)).ToString());
                case TokenType.DIVIDE:
                    return new Tree.Number((op1.Evaluate(this.env) / op2.Evaluate(this.env)).ToString());
                case TokenType.PLUS:
                    return new Tree.Number((op1.Evaluate(this.env) + op2.Evaluate(this.env)).ToString());
                case TokenType.MINUS:
                    return new Tree.Number((op1.Evaluate(this.env) - op2.Evaluate(this.env)).ToString());
                case TokenType.MODULUS:
                    return new Tree.Number((op1.Evaluate(this.env) % op2.Evaluate(this.env)).ToString());
                case TokenType.EQUAL_TO:
                    return new Tree.Boolean((op1.Evaluate(this.env) == op2.Evaluate(this.env)).ToString());
                case TokenType.NOT_EQUAL_TO:
                    return new Tree.Boolean((op1.Evaluate(this.env) != op2.Evaluate(this.env)).ToString());
                case TokenType.LESS_THAN:
                    return new Tree.Boolean((op1.Evaluate(this.env) < op2.Evaluate(this.env)).ToString());
                case TokenType.LESS_THAN_OR_EQUAL_TO:
                    return new Tree.Boolean((op1.Evaluate(this.env) <= op2.Evaluate(this.env)).ToString());
                case TokenType.GREATER_THAN:
                    return new Tree.Boolean((op1.Evaluate(this.env) > op2.Evaluate(this.env)).ToString());
                case TokenType.GREATER_THAN_OR_EQUAL_TO:
                    return new Tree.Boolean((op1.Evaluate(this.env) >= op2.Evaluate(this.env)).ToString());
                case TokenType.LOGICAL_AND:
                    var op1Res = op1.Evaluate(this.env);
                    var op2Res = op2.Evaluate(this.env);
                    return new Tree.Boolean((op1Res && op2Res).ToString());
                case TokenType.LOGICAL_OR:
                    var operationOneRes = op1.Evaluate(this.env); 
                    var operationTwoRes = op2.Evaluate(this.env);
                    return new Tree.Boolean((operationOneRes || operationTwoRes).ToString());
                default:
                    this.Error(operata.Type + " cannot be used as an expression operator", operata.Column, operata.Row);
                    return null;
            }
        }
    }
}