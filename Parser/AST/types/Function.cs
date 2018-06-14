using System;
using System.IO;
using System.Collections.Generic;

namespace Rook.Tree {
  public class Function: AST {

    private List<string> parameters;
    List<AST> bodyStatements = new List<AST>();
    public List<string> Parameters {
      get { return this.parameters; }
    }

    public Function(List<string> parameters, List<AST> bodyStatements = null): base(TreeType.FUNCTION) {
      this.parameters = parameters;
      if(bodyStatements != null)
        this.bodyStatements = bodyStatements;
    }
    public override dynamic Evaluate(Env.Environment env) {
      return this;
    }
    public virtual dynamic Solve(Env.Environment env) {
      dynamic result = null;
      foreach(AST line in this.bodyStatements) {
        result = line.Evaluate(env);
      }
      return result;
    }
  }
}