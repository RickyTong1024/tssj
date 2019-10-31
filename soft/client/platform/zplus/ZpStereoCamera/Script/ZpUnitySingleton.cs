using UnityEngine;
using ZpGame;
public class ZpUnitySingleton<T>
    where T : new()
{
    private static T _instance = default(T);
    private static object syncRoot = new Object();

    public static T Instance
    {
        get
        {
            lock (syncRoot)
            {
                if (_instance == null)
                {
                    _instance = ((default(T) == null) ? System.Activator.CreateInstance<T>() : default(T));
                }
            }
            return _instance;
        }
    }
}

public class ZpMonoUnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = default(T);
    private static object syncRoot = new Object();

    public static T Instance
    {
        get
       {
            if (applicationIsQuitting)
            {
                Debugger.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (syncRoot)  
            {  
                if (_instance == null)  
                {  
                    _instance = FindObjectOfType(typeof(T)) as T;  
  
                    if (FindObjectsOfType(typeof(T)).Length > 1)  
                    {  
                        Debugger.LogError("[Singleton] Something went really wrong " +  
                            " - there should never be more than 1 singleton!" +  
                            " Reopening the scene might fix it.");  
                        return _instance;  
                    }  
  
                    if (_instance == null)  
                    {  
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();  
                        singleton.name = "(singleton) " + typeof(T).ToString();


                        DontDestroyOnLoad(singleton);
                        singleton.hideFlags = HideFlags.HideAndDontSave;
                        //Debugger.Log("[Singleton] An instance of " + typeof(T) +  
                        //    " is needed in the scene, so '" + singleton +  
                        //    "' was created with DontDestroyOnLoad.");  
                    }  
                    else  
                    {  
                        //Debugger.Log("[Singleton] Using instance already created: " +  
                        //    _instance.gameObject.name);  
                    }

                    ZpGameCallback _CallBack = Object.FindObjectOfType<ZpGameCallback>();
                    if (_CallBack == null)
                    {
                        GameObject go = new GameObject("ZpGameCallback", typeof(ZpGameCallback));
                        GameObject.DontDestroyOnLoad(go);
                         //go.hideFlags = HideFlags.HideAndDontSave;
                    }
                }

                return _instance;  
            }  
        }  

    }
    public static bool applicationIsQuitting = false;
    /// <summary>  
    /// When Unity quits, it destroys objects in a random order.  
    /// In principle, a Singleton is only destroyed when application quits.  
    /// If any script calls Instance after it have been destroyed,  
    ///   it will create a buggy ghost object that will stay on the Editor scene  
    ///   even after stopping playing the Application. Really bad!  
    /// So, this was made to be sure we're not creating that buggy ghost object.  
    /// </summary>  
    public virtual void OnDestroy()
    {
        applicationIsQuitting = true;
    }

}
