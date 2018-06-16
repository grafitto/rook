using Rook.Tokenizer;

namespace Rook.Tree {
    public class ReAssignment : Assignment {
        public ReAssignment(string left, AST right) : base(left, right){}

        public override dynamic Evaluate(Env.Environment env) {
            dynamic value = this.right.Evaluate(env);
            if(this.right.Type == TreeType.EXPRESSION) 
                env.Change(this.left, value);
            else 
                env.Change(this.left, this.right);
            return value;
        }
    }
}