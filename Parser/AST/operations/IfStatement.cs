namespace Rook.Tree {
  public class IfStatement : AST {

    private AST condition;
    private AST thenBlock;
    private AST elseBlock = null;
    public IfStatement(AST condition, AST thenBlock, AST elseBlock = null) : base(TreeType.IF) {
      this.condition = condition;
      this.thenBlock = thenBlock;
      this.elseBlock = elseBlock;
    }
    public override dynamic Evaluate(Env.Environment env) {
      var res = new Tree.Boolean(condition.Evaluate(env).ToString()).Evaluate(env);
      if(res) {
        return thenBlock.Evaluate(env);
      } else {
        if(elseBlock != null) {
          return elseBlock.Evaluate(env);
        } else {
          return new Tree.Null().Evaluate(env);
        }
      }
    }
  }
}