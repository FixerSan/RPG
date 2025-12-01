using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    //private ObjectManager _object;
    private ResourceManager _resource;
    private UIManager _ui;
    private PoolManager _pool;
    private SceneManager _scene;
    private ScreenManager _screen;
    private CoroutineManager _coroutine;
    private InputManager _input;

    private static bool init = false;


    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static UIManager UI { get { return Instance?._ui; } }
    public static PoolManager Pool { get { return Instance?._pool; } }
    public static SceneManager Scene { get { return Instance?._scene; } }
    public static ScreenManager Screen { get { return Instance?._screen; } }
    public static InputManager Input { get { return Instance?._input; } }
    public static CoroutineManager Routine { get { return CoroutineManager.Instance; } }


    private void Awake()
    {
        Init();
        
    }

    private static void Init()
    {
        if (init) return;
        Instance._resource = new ResourceManager();
        Instance._ui = new UIManager();
        Instance._pool = new PoolManager();

        Instance._scene = new SceneManager();
        Instance._screen = new ScreenManager();
        Instance._coroutine = CoroutineManager.Instance;
        Instance._input = new InputManager();
        init = true;
    }
}
