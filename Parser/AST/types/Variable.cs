namespace Rook.Tree {
    public class Variable: AST {
        private string name;
        public dynamic Value;
        public Variable(string name) : base(TreeType.IDENTIFIER) {
            this.name = name;
        }
        public override dynamic Evaluate(Env.Environment env) {
            this.Value = env.Get(this.name);
            return this.Value;
        }
    }
}