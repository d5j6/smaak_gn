using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            //if (_instance == null)
            //{
            //    Debug.LogError("The singleton " + typeof(T).FullName + " doesn't have an instance yet!");
            //}

            //In order to use this in edit mode
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                    Debug.LogError(typeof(T).ToString() + " could not be found!");

                //if (_instance == null)
                //    _instance = (new GameObject(typeof(T).ToString()).AddComponent<T>());
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Trying to create 2 instances of the " + typeof(T).FullName + " singleton! Existing Object: " + _instance.gameObject.name + " Failed object: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
    }
 
    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}