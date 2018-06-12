namespace Rook.Tree {
    public class ReAssignment : Assignment {
        public ReAssignment(string left, AST right) : base(left, right){}

        public override dynamic Evaluate(Env.Environment env) {
            dynamic value = this.right.Evaluate(env);
            env.Change(this.left, value);
            return value;
        }
    }
}