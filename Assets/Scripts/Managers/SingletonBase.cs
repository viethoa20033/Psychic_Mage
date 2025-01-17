using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject($"{typeof(T)} (Singleton)");
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);

                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}