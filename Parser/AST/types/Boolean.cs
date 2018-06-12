using System;

namespace Rook.Tree {
    public class Boolean: AST {

        public string value;
        public bool Value {
            get { 
                if(this.value.Equals("False") || this.value.Equals("")) {
                    return false;
                } else if(this.value.Equals("True")) {
                    return true;
                } else {
                    return true;
                }
            }
        }
        public Boolean(string value): base (TreeType.BOOLEAN)
        {
            this.value = value;
        }

        public override dynamic Evaluate(Env.Environment env) {
            return this.Value;
        }
    }
}