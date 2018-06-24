using System;

namespace Rook.Errors {
	public class RuntimeError : Exception {
		public RuntimeError() : base() {}
		public RuntimeError(string message) : base(message) {} 
		public RuntimeError(string message, Exception inner) : base(message, inner){}
	}
}