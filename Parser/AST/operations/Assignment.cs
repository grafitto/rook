using System.Collections.Generic;

namespace Rook.Tree {
    public class Assignment : AST {

        protected string left;
        protected AST right;
        public Assignment(string left, AST right) : base(TreeType.ASSIGNMENT) {
            this.right = right;
            this.left = left;
        }

        public override dynamic Evaluate(Env.Environment env) {
            if(this.right.Type == TreeType.LIST) {
                Tree.List right = this.right as List;
                List<AST> items = new List<AST>();
                foreach (AST item in right.Items) {
                    items.Add(item.Evaluate(env));
                }
                this.right = new Tree.List(items);
            } 
            env.Set(this.left, this.right.Evaluate(env));
            return this.right;
        }
    }
}