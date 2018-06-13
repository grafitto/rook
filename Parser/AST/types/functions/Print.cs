using System;
using System.IO;

namespace Rook.Tree {
  public class Print: Function {

    public Print(string[] parameters): base(parameters) {
    }
    public override dynamic Evaluate(Env.Environment env) {
      //We only expect one argument 
      Env.Environment environ = Env.Environment.Scope(env);
      AST value = environ.Get(this.Parameters[0]);
      dynamic str = value.Evaluate(env);
      using(StreamWriter writer = new StreamWriter(Console.OpenStandardOutput())) {
        writer.Write(str.ToString() + "\n");
      }
      return str;
    }
  }
}