using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<T>(typeof(T).Name);

                if (_instance == null)
                {
                    Debug.LogError("No se encontró el asset en Resources para tipo " + typeof(T));
                }
            }

            return _instance;
        }
    }
}