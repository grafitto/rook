using System;
using System.IO;
using Rook.Errors;
using System.Collections.Generic;

namespace Rook.Tree {
  public class List: AST {

      private string print;
    private List<AST> value;
    public List<AST> Value { get { return this.value; } }  
    private List<AST> items = new List<AST>();
    public List<AST> Items {
      get { return this.items; }
    }

    public List(List<AST> items): base(TreeType.LIST) {
      this.items = items;
    }
    public override dynamic Evaluate(Env.Environment env) {
        this.print = "[";
        foreach(AST item in this.Items) {
            this.print += item.Evaluate(env).Value.ToString();
            this.print += ",";
        }
        this.print += "]";
        return this;
    }
    public virtual dynamic Solve(Env.Environment env) {
        return 0;
    }
    public AST Get(double index) {
        int i = int.Parse(index.ToString());
        if(i < 0 || i > items.Count) {
            throw new RuntimeError("Index was out of range.");
        }
        return this.items[i];
    }
    public AST  Insert(double index, dynamic value) {
        int i = int.Parse(index.ToString());
        this.items[i] = value;
        return value;
    }
    public dynamic Add(dynamic value) {
        this.items.Add(value);
        return value;
    }
    public Number Length() {
        return new Tree.Number(this.Items.Count);
    }
    public Tree.List Sort() {
        //List<AST> items = this.items.Sort(IComparer<AST>{})
        return new Tree.List(items);
    }
    public override string ToString() {
        return this.print;
    }
  }
}