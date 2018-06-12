using System;
using System.Collections.Generic;

namespace Rook.Tree.Env {
    public class Environment {

        private Dictionary<string, dynamic> env;
        public Environment() {
            this.env = new Dictionary<string, dynamic>();
        }
        public void Set(string name, dynamic value) {
            if(this.env.ContainsKey(name))
                throw new Exception("Another variable " + name + " has already been initialized.");
            else
                this.env.Add(name, value);
        }
        public void Change(string name, dynamic value) {
            if(this.env.ContainsKey(name))
                this.env[name] = value;
            else
                throw new Exception("Variable " + name + " has not been initialized.");
        }
        public dynamic Get(string name) {
            if(this.env.ContainsKey(name)) {
                return this.env[name];
            } else {
                throw new Exception("Variable not initialized.");
            }
        }
    }
}