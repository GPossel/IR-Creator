using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; set; }
    protected virtual void Awake()
    {
        Instance = this as T;
    }

    private void Start()
    {
        if (Instance != null) { Destroy(this); }
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }

}

public abstract class SingletonDontDestroyOnLoad<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null) { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}
