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
        public Boolean(dynamic value): base (TreeType.BOOLEAN)
        {
            Boolean b = value as Boolean;
            if(b != null){
                bool bValue = b.Value;
                if(bValue) {
                    this.value = "True";
                } else {
                    this.value = "False";
                }
            } else {
                this.value = value;
            }
            
        }

        public override dynamic Evaluate(Env.Environment env) {
            return this;
        }
    }
}