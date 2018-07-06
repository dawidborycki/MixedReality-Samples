#region Using

using System;

#endregion

namespace MixedReality.Common.ArgumentValidation
{
    public static class Check
    {
        #region Methods (Public)

        public static void IsNull(object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        #endregion
    } 
}
