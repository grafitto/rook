using System;
using System.IO;
using System.Collections.Generic;

namespace Rook.Tree {
  public class Print: Function {

    public Print(List<string> parameters): base(parameters) {
    }
    public override dynamic Evaluate(Env.Environment env) {
      //We only expect one argument 
      return this;
    }
    public override dynamic Solve(Env.Environment env) {
      Env.Environment environ = Env.Environment.Scope(env);
      AST value = environ.Get(this.Parameters[0]);
      dynamic print = null;
      if(value.Type == TreeType.LIST) {
        print = value.Evaluate(environ).ToString();
      } else {
        print = value.Evaluate(environ).Value;
      }
      using(StreamWriter writer = new StreamWriter(Console.OpenStandardOutput())) {
        writer.Write(print.ToString() + "\n");
      }
      return value;
    }
  }
}