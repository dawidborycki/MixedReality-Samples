#region Using

using UnityEngine;

#endregion

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    #region Fields

    private static T instance;

    #endregion

    #region Properties

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }

    #endregion
}
