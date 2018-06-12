namespace Rook.Tree {
    public class Assignment : AST {

        protected string left;
        protected AST right;
        public Assignment(string left, AST right) : base(TreeType.ASSIGNMENT) {
            this.right = right;
            this.left = left;
        }

        public override dynamic Evaluate(Env.Environment env) {
            dynamic value = this.right.Evaluate(env);
            env.Set(this.left, value);
            return value;
        }
    }
}