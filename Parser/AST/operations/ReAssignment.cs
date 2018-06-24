using Rook.Tokenizer;

namespace Rook.Tree {
    public class ReAssignment : Assignment {
        public ReAssignment(string left, AST right) : base(left, right){}

        public override dynamic Evaluate(Env.Environment env) {
            if(this.right.Type == TreeType.LIST_MODIFY) {
                ListModify modifier = this.right as ListModify;
                env.ChangeList(left, modifier.Index.Evaluate(env).Value, modifier.Right.Evaluate(env));
                return modifier.Right;
            }
            dynamic value = this.right.Evaluate(env);
            if(this.right.Type == TreeType.EXPRESSION) { 
                env.Change(this.left, value);
            } else { 
                env.Change(this.left, this.right);
            }
            return value;
        }
    }
}