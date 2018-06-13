using System;
using System.IO;
using System.Collections.Generic;

namespace Rook.Tree {
  public class Function: AST {

    private string[] parameters;
    List<AST> bodyStatements;
    public string[] Parameters {
      get { return this.parameters; }
    }

    public Function(string[] parameters, List<AST> bodyStatements = null): base(TreeType.FUNCTION) {
      this.parameters = parameters;
      this.bodyStatements = bodyStatements;
    }
    public override dynamic Evaluate(Env.Environment env) {
      dynamic result = null;
      foreach(AST line in this.bodyStatements) {
        result = line.Evaluate(env);
      }
      return result;
    }
  }
}