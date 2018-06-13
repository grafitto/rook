namespace Rook.Tree {
    public enum TreeType {
        ASSIGNMENT,
        NUMBER,
        STRING,
        IDENTIFIER,
        EXPRESSION,
        NULL,
        BOOLEAN,
        IF,
        FUNCTION_CALL,
        FUNCTION,

        //INBUILT FUNCTIONS
        INBUILT_PRINT_FUNCTION,
        INBUILT_READ_FUNCTION
    }
}