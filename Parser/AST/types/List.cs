using System;
using System.IO;
using System.Collections.Generic;

namespace Rook.Tree {
  public class List: AST {

    private List<AST> items;
    public List<AST> Items {
      get { return this.items; }
    }

    public List(List<AST> items): base(TreeType.LIST) {
      this.items = items;
    }
    public override dynamic Evaluate(Env.Environment env) {
      return this;
    }
    public virtual dynamic Solve(Env.Environment env) {
      return 0;
    }
  }
}