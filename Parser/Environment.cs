using System;
using Rook.Errors;
using System.Collections.Generic;

namespace Rook.Tree.Env {
    public class Environment {
        private static Environment scope = null;
        private Dictionary<string, dynamic> env;
        private Environment parent;
        public Dictionary<string, dynamic> Env {
            get { return this.env; }
        }
        private Environment(Environment parent) {
            this.parent = parent;
            this.env = new Dictionary<string, dynamic>();
        }
        public void Set(string name, dynamic value) {
            if(this.env.ContainsKey(name))
                throw new RuntimeError("Another variable " + name + " has already been initialized.");
            else
                this.env.Add(name, value);
        }
        public void Change(string name, dynamic value) {
            if(this.env.ContainsKey(name))
                this.env[name] = value;
            else
                throw new RuntimeError("Variable " + name + " has not been initialized.");
        }
        public void ChangeList(string name, double index, dynamic value) {
            int i = int.Parse(index.ToString());
            if(this.env.ContainsKey(name)) {
                this.env[name].Insert(i, value);
            } else {
                throw new RuntimeError("List " + name + " not initialized.");
            }
        }
        public dynamic Get(string name) {
            if(this.env.ContainsKey(name)) {
                // Chech the current scope
                return this.env[name];
            } else if(this.parent != null) {
                // Check the parent scope
                return this.parent.Get(name);
            } else {
                throw new RuntimeError("Variable " + name + " not initialized.");
            }
        }
        public static Environment Scope(Environment parent = null) {
            return new Environment(parent);
        }
    }
}