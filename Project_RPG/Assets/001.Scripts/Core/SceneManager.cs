using System;
using UnityEngine;

/// <summary>
/// 씬 관리자 클래스 - 씬 전환 및 씬 기반 객체 관리를 담당
/// </summary>
public class SceneManager
{
    // 씬 기반 객체들을 담을 Transform (DontDestroyOnLoad로 유지)
    private Transform sceneTrans;

    /// <summary>
    /// 씬 기반 객체들을 담을 Transform을 반환하는 프로퍼티 (없으면 생성)
    /// </summary>
    public Transform SceneTrans
    {
        get
        {
            if (sceneTrans == null)
            {
                GameObject go = GameObject.Find("@SceneTrans");
                if (go == null)
                    go = new GameObject(name: "@SceneTrans");
                sceneTrans = go.transform;
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
            return sceneTrans;
        }
    }

    // 현재 활성화된 씬
    private Define.Scene currentScene;

    /// <summary>
    /// 현재 활성화된 씬을 반환하는 프로퍼티
    /// </summary>
    public Define.Scene CurrentScene
    {
        get { return currentScene; }
    }

    // 씬 로딩 중인지 여부
    private bool isLoading = false;

    // 씬 로딩 완료 시 호출할 콜백
    private Action loadCallback;

    /// <summary>
    /// 씬을 비동기로 로드하는 함수
    /// </summary>
    /// <param name="_scene">로드할 씬</param>
    /// <param name="_isHaveFade">페이드 효과 사용 여부 (기본값: true)</param>
    /// <param name="_loadCallback">로딩 완료 시 호출할 콜백</param>
    public void LoadScene(Define.Scene _scene, bool _isHaveFade = true, Action _loadCallback = null)
    {
        if (!_isHaveFade)
        {
            if (isLoading) return;
            isLoading = true;
            loadCallback = _loadCallback;

            Managers.Pool.Clear();
            Managers.UI.CloseAllPopupUI();

            RemoveScene(currentScene, () =>
            {
                currentScene = _scene;
                AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync($"{_scene}");
                async.completed += (_) =>
                {
                    AddScene(_scene, () => { loadCallback?.Invoke(); });
                    isLoading = false;
                };
            });
            return;
        }

        Managers.Screen.FadeIn(0.25f, () =>
        {
            if (isLoading) return;
            isLoading = true;
            loadCallback = _loadCallback;

            Managers.Pool.Clear();
            Managers.UI.CloseAllPopupUI();
            RemoveScene(currentScene, () =>
            {
                currentScene = _scene;
                AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync($"{_scene}");
                async.completed += (_) =>
                {
                    AddScene(_scene, () =>
                    {
                        isLoading = false;
                        loadCallback?.Invoke();
                        Managers.Screen.FadeOut(0.25f);
                    });
                };
            });
        });
    }

    /// <summary>
    /// 현재 씬을 제거하는 함수 (씬 전환 전 정리 작업)
    /// </summary>
    /// <param name="_scene">제거할 씬</param>
    /// <param name="_callback">제거 완료 시 호출할 콜백</param>
    public void RemoveScene(Define.Scene _scene, Action _callback = null)
    {
        SceneBase bs = null;
        switch (_scene)
        {
            default:
                _callback?.Invoke();
                return;
        }

        if (bs != null)
        {
            bs?.Clear();
            UnityEngine.Object.Destroy(bs);
        }
        _callback?.Invoke();
    }

    /// <summary>
    /// 새로운 씬을 추가하는 함수 (씬 전환 후 초기화 작업)
    /// </summary>
    /// <param name="_scene">추가할 씬</param>
    /// <param name="_addSceneCallback">추가 완료 시 호출할 콜백</param>
    public void AddScene(Define.Scene _scene, Action _addSceneCallback)
    {
        SceneBase bs = null;
        switch (_scene)
        {
            //case Define.Scene.Stage:
            //    bs = SceneTrans.gameObject.AddComponent<StageScene>();
            //    break;
        }

        bs?.Init();
        _addSceneCallback?.Invoke();
    }

    /// <summary>
    /// 현재 활성화된 씬을 특정 타입으로 반환하는 함수
    /// </summary>
    /// <typeparam name="T">반환할 씬 타입 (SceneBase를 상속받아야 함)</typeparam>
    /// <returns>씬 인스턴스 (없으면 null)</returns>
    public T GetActiveScene<T>() where T : SceneBase
    {
        return SceneTrans.GetComponent<T>() as T;
    }
}
