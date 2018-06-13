using System;
using System.Collections.Generic;

namespace Rook.Tree {
  public class FunctionCall: AST {

    List<AST> arguments;
    string name;
    public FunctionCall(string name, List<AST> arguments) : base(TreeType.FUNCTION_CALL) {
      this.arguments = arguments;
      this.name = name;
    }
    public override dynamic Evaluate(Env.Environment env) {
      /**
        * Should create a new environment scope
        * Assign all arguments to function paramenter names in order
        * Store them in the new Environment scope
        * Evaluate the function from the environment passing the new scope
       */
       Env.Environment scope = Env.Environment.Scope(env);
       Function function = env.Get(this.name);
       if(this.arguments.Count != function.Parameters.Length){
         throw new Exception("Expected " + function.Parameters.Length  + " to function " + this.name + " call. Found " + this.arguments.Count);
       }
       for(int i = 0; i < this.arguments.Count; i++) {
         scope.Set(function.Parameters[i], this.arguments[i]);
       }
      return function.Evaluate(scope);
    }
  }
}