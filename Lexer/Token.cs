namespace Rook.Tokenizer {

    public class Token {
        private TokenType type;
        private string value;

        private int column;
        private int row;

        public int Column {
            get { return this.column; }
        }
        public int Row {
            get { return this.row; }
        }
        public TokenType Type {
            get { return this.type; }
        }
        public string Value {
            get { return this.value; }
        }


        public Token(TokenType type, string value, int column, int row) {
            this.type = type;
            this.value = value;
            this.column = column;
            this.row = row;
        }

        public bool Is(TokenType type)
        {
            return this.type == type;
        }
        public bool isOperator()
        {
            switch(this.type)
            {
                case TokenType.PLUS:
                    return true;
                case TokenType.MINUS:
                    return true;
                case TokenType.DIVIDE:
                    return true;
                case TokenType.TIMES:
                    return true;
                case TokenType.LESS_THAN:
                    return true;
                case TokenType.EQUAL_TO:
                    return true;
                case TokenType.NOT_EQUAL_TO:
                    return true;
                case TokenType.GREATER_THAN:
                    return true;
                case TokenType.LESS_THAN_OR_EQUAL_TO:
                    return true;
                case TokenType.GREATER_THAN_OR_EQUAL_TO:
                    return true;
                case TokenType.MODULUS:
                    return true;
                case TokenType.LOGICAL_AND:
                    return true;
                case TokenType.LOGICAL_OR:
                    return true;
                case TokenType.LEFT_BRACKET:
                    return true;
                case TokenType.RIGHT_BRACKET:
                    return true;
                default:
                    return false;
            }
        }
        public override string ToString() {
            return "Token( " + this.type + " , " + this.value + " , " + this.column + ":" + this.row + " )";
        }
    }
}