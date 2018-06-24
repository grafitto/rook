namespace Rook.Tree {
  public class ListAccess : AST {

    private string listName;
    private AST index;
    public dynamic Value;

    public ListAccess(string listName, AST index) : base(TreeType.LIST_ACCESS) {
      this.listName = listName;
      this.index = index;
    }

    public override dynamic Evaluate(Env.Environment env) {
      Tree.List list = env.Get(this.listName);
      int index = int.Parse(this.index.Evaluate(env).Value.ToString());
      this.Value = list.Get(index);
      return this.Value;
    }
  }
}