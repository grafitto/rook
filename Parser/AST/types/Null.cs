using System;

namespace Rook.Tree {
    public class Null: AST {

        public string value = "Null";
        public string Value { get { return this.value; } }
        public Null(): base (TreeType.NULL){}
        public override dynamic Evaluate(Env.Environment env) {
            return this;
        }
    }
}