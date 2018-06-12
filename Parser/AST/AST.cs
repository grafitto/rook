using Rook.Tree.Env;

namespace Rook.Tree {
    public abstract class AST {
        protected TreeType type;

        public TreeType Type {
            get { return this.type; }
        }
        /**
        * This holds as the parent of all types of Abstract syntax trees
        * Also has the tree type
        */
        public AST(TreeType type) {
            this.type = type;
        }
        public void Error(string msg, int column, int row)
        {
            throw new System.Exception(msg + " (" + column + "," + row + ")");
        }
        public abstract dynamic Evaluate(Environment env);
    }
}