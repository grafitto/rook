using System.Collections.Generic;
using Rook.Tokenizer;
using System;
using System.Reflection;

namespace Rook.Tree
{
    public class Expression : AST
    {
        public  dynamic Value;
        List<dynamic> list;
        Env.Environment env;
        Dictionary<TokenType,int> PRECEDENCE = new Dictionary<TokenType, int>();
        public Expression(List<dynamic> list) : base(TreeType.EXPRESSION)
        {
            this.list = list;
            this.PRECEDENCE.Add(TokenType.LEFT_BRACKET, 20);
            this.PRECEDENCE.Add(TokenType.RIGHT_BRACKET, 20);
            this.PRECEDENCE.Add(TokenType.FULL_STOP, 19);
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
            dynamic result = Solve(CreatePostFix()) ;
            FunctionCall call = result as FunctionCall;
            if(call != null) {
                this.Value = call.Evaluate(env);
            } else {
                this.Value = result;
            }
            return this.Value;
        }
        public List<dynamic> CreatePostFix()
        {
            List<dynamic> postfix = new List<dynamic>();
            Stack<Token> operatorStack = new Stack<Token>();
            foreach(dynamic item in this.list)
            {
                Token operata = item as Token;
                if(operata != null) {
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
                } else {
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
                Token operata = item as Token;
                if(operata != null) {
                    AST op2 = stack.Pop();
                    AST op1 = stack.Pop();
                    stack.Push(this.Apply(operata, op1, op2)); 
                } else {
                     AST oparand = item;
                    stack.Push(oparand);
                    
                }
            }
            return stack.Pop();
        }
        public AST Apply(Token operata, AST op1, AST op2)
        {
            op1 = this.Convert(op1);
            if(!operata.Is(TokenType.FULL_STOP)) {
                op2 = this.Convert(op2);
            }
            switch(operata.Type)
            {
                case TokenType.TIMES:
                    return new Tree.Number((op1.Evaluate(this.env).Value * op2.Evaluate(this.env).Value).ToString());
                case TokenType.DIVIDE:
                    return new Tree.Number((op1.Evaluate(this.env).Value / op2.Evaluate(this.env).Value).ToString());
                case TokenType.PLUS:
                    double op1res = op1.Evaluate(this.env).Value;
                    double op2res = op2.Evaluate(this.env).Value;
                    return new Tree.Number((op1res + op2res).ToString());
                case TokenType.MINUS:
                    return new Tree.Number((op1.Evaluate(this.env).Value - op2.Evaluate(this.env).Value).ToString());
                case TokenType.MODULUS:
                    return new Tree.Number((op1.Evaluate(this.env).Value % op2.Evaluate(this.env).Value).ToString());
                case TokenType.EQUAL_TO:
                    return new Tree.Boolean((op1.Evaluate(this.env).Value == op2.Evaluate(this.env).Value).ToString());
                case TokenType.NOT_EQUAL_TO:
                    return new Tree.Boolean((op1.Evaluate(this.env).Value != op2.Evaluate(this.env).Value).ToString());
                case TokenType.LESS_THAN:
                    return new Tree.Boolean((op1.Evaluate(this.env).Value < op2.Evaluate(this.env).Value).ToString());
                case TokenType.LESS_THAN_OR_EQUAL_TO:
                    return new Tree.Boolean((op1.Evaluate(this.env).Value <= op2.Evaluate(this.env).Value).ToString());
                case TokenType.GREATER_THAN:
                    return new Tree.Boolean((op1.Evaluate(this.env).Value > op2.Evaluate(this.env).Value).ToString());
                case TokenType.GREATER_THAN_OR_EQUAL_TO:
                    return new Tree.Boolean((op1.Evaluate(env).Value >= op2.Evaluate(env).Value).ToString());
                case TokenType.LOGICAL_AND:
                    var op1Res = op1.Evaluate(this.env).Value;
                    var op2Res = op2.Evaluate(this.env).Value;
                    return new Tree.Boolean((op1Res && op2Res).ToString());
                case TokenType.LOGICAL_OR:
                    var operationOneRes = op1.Evaluate(this.env).Value; 
                    var operationTwoRes = op2.Evaluate(this.env).Value;
                    return new Tree.Boolean((operationOneRes || operationTwoRes).ToString());
                case TokenType.FULL_STOP:
                    return this.AccessModifier(op1, op2);
                default:
                    this.Error(operata.Type + " cannot be used as an expression operator", operata.Column, operata.Row);
                    return null;
            }
        }
        public AST AccessModifier(AST callee, AST modifier) {
            Type called = callee.GetType();
            string calledName = this.Capitalize(callee.Type.ToString());
            FunctionCall fcall = modifier as FunctionCall;
            Variable vCallee = callee as Variable;
            if(vCallee != null) {
                callee = vCallee.Evaluate(this.env);
                called = callee.GetType();
            }
            if(fcall != null){
                // Its a function call with params
                string fname = fcall.name;
                object[] parameters = new object[fcall.arguments.Count];
                for(int i = 0; i < fcall.arguments.Count; i++) {
                    parameters[i] = new object();
                    parameters[i] = fcall.arguments[i].Evaluate(this.env);
                }
                return (AST)called.InvokeMember(fname, BindingFlags.InvokeMethod, null, callee, parameters);
            } else {
                Variable fname = modifier as Variable;
                return (AST)called.InvokeMember(fname.name, BindingFlags.InvokeMethod, null, callee, null);
            }
         }
        public string Capitalize(string s) {       
            return s[0].ToString().ToUpper() + s.Remove(0, 1).ToLower();
        }
        private AST Convert(AST candidate) {
            switch(candidate.Type) {
                case TreeType.IDENTIFIER:
                    return candidate.Evaluate(this.env);
                case TreeType.FUNCTION_CALL:
                    return candidate.Evaluate(this.env);
                default:
                    return candidate;
            }
        }
    }
}