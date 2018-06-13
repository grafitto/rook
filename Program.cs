﻿using System;
using Rook.Tree;
using System.IO;
using Rook.Parse;
using Rook.Tree.Env;
using Rook.Tokenizer;
using System.Collections.Generic;

namespace Rook
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = File.ReadAllText("Test.rk");
            Lexer lexer = new Lexer(code); 

            /* Token next = lexer.Next();
            while(next.Type != TokenType.EOF) {
                Console.WriteLine(next);
                next = lexer.Next();
            } */
            Parser parser = new Parser(code);
            List<AST> trees = parser.Parse();

            Tree.Env.Environment env = Tree.Env.Environment.Scope();
            env.Set("print", new Print(new string[]{"value"}));
            foreach(AST tree in trees) {
                tree.Evaluate(env);
            }
            /* while(token.Type != TokenType.EOF) {
                Console.WriteLine(token);
                token = lexer.Next();
            } */

            /* string n = ".345";
    
            double result;
            Console.WriteLine(n);
            double.TryParse(n, out result);
            Console.WriteLine(result); */
        }
    }
}
