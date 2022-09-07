using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{

    private static T instance;

    public static T Instance
    {
        get{ return instance; }
    }

    ///////////////////////////////////////////

    public static bool IsInitalized()
    {
        return instance != null;
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }


}
