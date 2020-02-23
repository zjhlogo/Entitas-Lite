using System;

namespace Entitas
{
	/// <summary>
	/// Base exception used by Entitas.
	/// </summary>
	public class EntitasException : Exception
	{
		public EntitasException(string message, string hint = null)
			: base(hint != null ? $"{message}\n{hint}" : message)
		{
		}
	}
}