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
            return this;
        }
        public String ToUpper() {
            return new Tree.String(this.value.ToUpper());
        }
        public String Concat(dynamic other) {
            string value = other.Value.ToString();
            return new Tree.String(this.Value + value);
        }
        public String ToLower() {
            return new Tree.String(this.Value.ToLower());
        }
        public String Capitalize() {
            return new Tree.String(this.Capitalize(this.Value));
        }
        public Number ToNumber() {
           return new Tree.Number(this.value);
        }
        private string Capitalize(string s) {       
            return s[0].ToString().ToUpper() + s.Remove(0, 1).ToLower();
        }
    }
}