using System;

namespace Rook.Tree {
    public class String: AST {

        public string value;
        public string Value {
            get { 
                return this.value;
            }
        }
        public String(string value): base (TreeType.STRING)
        {
            this.value = value;
        }

        public override dynamic Evaluate(Env.Environment env) {
            return this.Value;
        }
    }
}