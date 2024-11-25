﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Exceptions
{
    public class ArgumentsValidator
    {
        private static void HandleException(string argName, object argValue)
        {
            if (string.IsNullOrWhiteSpace(argName)) throw new ArgumentNullException(argName, innerException: new Exception($"Argument '{argValue}' cannot be null or empty"));
        }
        public static void ThrowIfNull(string argName, object argValue) => HandleException(argName, argValue);
    }
}
