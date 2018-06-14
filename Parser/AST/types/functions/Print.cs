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
      dynamic value = environ.Get(this.Parameters[0]);
      using(StreamWriter writer = new StreamWriter(Console.OpenStandardOutput())) {
        writer.Write(value.ToString() + "\n");
      }
      return value;
    }
  }
}