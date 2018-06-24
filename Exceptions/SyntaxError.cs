using System;

namespace Rook.Errors {
	public class SyntaxError : Exception {
		public SyntaxError() : base() {}
		public SyntaxError(string message) : base(message) {} 
		public SyntaxError(string message, Exception inner) : base(message, inner){}
	}
}