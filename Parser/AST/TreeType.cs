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

        LIST,

        //INBUILT FUNCTIONS
        INBUILT_PRINT_FUNCTION,
        INBUILT_READ_FUNCTION,
        LIST_ACCESS,
        LIST_MODIFY
    }
}