using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using ErrorCode = Odyssey.Properties.Errors;

namespace Odyssey.Engine
{
    public static class Error
    {
        internal static string IndexNotInRange(string varName, string arrayName, int value)
        {
            return string.Format(ErrorCode.ERR_IndexNotInRange, varName, arrayName, value);
        }
        
        #region Arguments

        public static string ArgumentInvalid(string argumentName, object value)
        {
            return string.Format(ErrorCode.ERR_Argument, argumentName, value);
        }

       
        public static ArgumentException ArgumentInvalid(string argument, Type type, string method, string message = null, string objValue = null)
        {
            if (message == null)
                message = ErrorCode.ERR_NoDetails;
            if (message != null && objValue != null)
                message = string.Format(message, objValue);

            return new ArgumentException(string.Format(ErrorCode.ERR_Argument, argument, type.Name, method),
                argument, new Exception(message));
        }

        public static ArgumentNullException ArgumentNull(string argument, Type type, string method, string message = null)
        {
            if (message == null)
                message = ErrorCode.ERR_NoDetails;

            return new ArgumentNullException(string.Format(ErrorCode.ERR_ArgumentNull, argument, type.Name, method),
                new ArgumentException(message, argument));
        }

        public static ArgumentException InvalidEnum(string argument, Type type, string method, string message = null, string objValue = null)
        {
            if (message == null)
                message = ErrorCode.ERR_NoDetails;
            if (message != null && objValue != null)
                message = string.Format(message, objValue);

            return new ArgumentException(string.Format(ErrorCode.ERR_EnumNotValid, argument, type.Name, method),
                argument, new Exception(message));
        }

        public static ArgumentNullException InCreatingFromObject(string paramName, Type instance, Type argument)
        {
            return new ArgumentNullException(paramName,
                    string.Format(CultureInfo.CurrentCulture,
                        ErrorCode.ERR_CreatingFromObject,
                        instance.Name, argument.Name));
        }

        public static InvalidOperationException WrongCase(string param, string method, object value)
        {
            return new InvalidOperationException(string.Format(ErrorCode.ERR_WrongCase, param, method));
        }

        #endregion Arguments

        public static KeyNotFoundException KeyNotFound(string key, string collection, string message = null)
        {
            if (message == null)
                message = ErrorCode.ERR_KeyNotFound;

            return new KeyNotFoundException(string.Format(message, collection, key));
        }

        internal static NotSupportedException NotSupported(string message)
        {
            return new NotSupportedException(message);
        }

        public static InvalidOperationException InvalidOperation(string message)
        {
            return new InvalidOperationException(message);
        }
    }
}