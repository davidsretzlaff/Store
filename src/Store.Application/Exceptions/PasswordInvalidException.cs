using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Exceptions
{
	internal class PasswordInvalidException : ApplicationException
	{
		public PasswordInvalidException(string? message) : base(message)
		{

		}

		public static void ThrowIfPasswordInvalid()
		{
			throw new PasswordInvalidException("Password Invalid");
		}
	}
}
