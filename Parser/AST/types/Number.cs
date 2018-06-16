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
        public Number(double value) : base (TreeType.NUMBER) {
            this.value = value.ToString();
        }

        public override dynamic Evaluate(Env.Environment env) {
            return this;
        }
        public Number Negate() {
            return new Number(-this.Value);
        }
        public Number Sqr() {
            return new Number(this.Value * this.Value);
        }
        public Number Sqrt() {
            return new Number(Math.Sqrt(this.Value));
        }
        public Number Cos() {
            return new Number(Math.Cos(this.Value));
        }
        public Number Sin() {
            return new Number(Math.Sin(this.Value));
        }
        public Number Pow(dynamic x) {
            x = x.Value;
            return new Number(Math.Pow(this.Value, x));
        }
        public Number Log(dynamic b = null) {
            if(b != null) b = b.Value;
            return new Number(Math.Log(this.Value, b));
        }
        public Number Exp() {
            return new Number(Math.Exp(this.Value));
        }
    }
}