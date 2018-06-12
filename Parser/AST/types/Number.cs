using System;

namespace Rook.Tree {
    public class Number: AST {

        public string value;
        public double Value {
            get { 
                try {
                    return double.Parse(this.value);
                } catch (Exception) {
                    throw new Exception("Number could not be converted to Double");
                }
            }
        }
        public Number(string value): base (TreeType.NUMBER)
        {
            this.value = value;
        }

        public override dynamic Evaluate(Env.Environment env) {
            return this.Value;
        }
    }
}