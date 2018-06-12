using System;

namespace Rook.Tokenizer {
    public class InputStream {
        private int column = 0;
        private int position = 0;
        private int line = 1;
        public int Column { get { return this.column; } }
        public int Row { get { return this.line; } }
        private string code;
        public InputStream(string code) {
            this.code = code;
        }
        /*
            This returns the character at the current position
        */
        public char Next() {
            var character = code[position++];
            if(character.Equals('\n')) {
                line++;
                column = 0;
            } else {
                column++;
            }
            return character;
        }

        /* 
            This returns the next character
        */
        public char Peek(int ahead = 0) {
            return code[position + ahead];
        }
        //Checks whether we are out of characters
        public bool End() {
            return position >= code.Length;
        }
        public void Previous() {
            position--;
        }
        //Manages errors
        public void Error(string msg) {
            throw new Exception(msg + "(" + line + "," + column + ")");
        }
    }
}