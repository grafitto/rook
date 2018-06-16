namespace Rook.Tree {
  public class IfStatement : AST {

    public AST Value;
    private AST condition;
    private AST thenBlock;
    private AST elseBlock = null;
    public IfStatement(AST condition, AST thenBlock, AST elseBlock = null) : base(TreeType.IF) {
      this.condition = condition;
      this.thenBlock = thenBlock;
      this.elseBlock = elseBlock;
    }
    public override dynamic Evaluate(Env.Environment env) {
      var res = new Tree.Boolean(condition.Evaluate(env)).Value;
      if(res) {
        this.Value = thenBlock.Evaluate(env);
        return this.Value;
      } else {
        if(elseBlock != null) {
          this.Value = elseBlock.Evaluate(env);
          return this.Value;
        } else {
          this.Value = new Tree.Null();
          return this.Value;
        }
      }
    }
  }
}