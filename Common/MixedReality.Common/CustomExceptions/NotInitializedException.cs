#region Using

using System;

#endregion

namespace MixedReality.Common.CustomExceptions
{
    public class NotInitializedException : Exception
    {
        #region Constructors

        public NotInitializedException()
        {
        }

        public NotInitializedException(string message) : base(message)
        {
        }

        public NotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}