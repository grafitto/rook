namespace Rook.Tree {
  public class ListModify : AST {

    private string listName;
    private AST index;
    public AST Index { get { return this.index; } }
    private AST right;
    public AST Right { get { return this.right; } }
    private dynamic Value;

    public ListModify(string listName, AST index, AST right) : base(TreeType.LIST_MODIFY) {
      this.listName = listName;
      this.index = index;
      this.right = right;
    }

    public override dynamic Evaluate(Env.Environment env) {
      Tree.List list = env.Get(this.listName);
      this.Value = list.Get(this.index.Evaluate(env).Value);
      return this.Value;
    }
  }
}