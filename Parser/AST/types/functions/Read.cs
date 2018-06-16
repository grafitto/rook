using System;
using System.IO;
using System.Collections.Generic;

namespace Rook.Tree {
  public class Read: Function {

    public Read(List<string> parameters): base(parameters) {
    }
    public override dynamic Evaluate(Env.Environment env) {
      //We only expect one argument 
      return this;
    }
    public override dynamic Solve(Env.Environment env) {
      Env.Environment environ = Env.Environment.Scope(env);
      dynamic value = environ.Get(this.Parameters[0]);
      string input = "";
      using(StreamWriter writer = new StreamWriter(Console.OpenStandardOutput())) {
        writer.Write(value.Value.ToString());
      }
      using(StreamReader reader = new StreamReader(Console.OpenStandardInput())) {
        input = reader.ReadLine();
        reader.Close();
      }
      return new Tree.String(input);
    }
  }
}