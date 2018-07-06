#region Using

using System;
using Urho;
using Windows.ApplicationModel.Core;

#endregion

namespace ExploringUrhoSharp
{
    public static class Program
    {
        #region Main
        
        [MTAThread]
        static void Main() => CoreApplication.Run(
            new UrhoAppViewSource<MixedRealityApplication>());        

        #endregion
    }
}
