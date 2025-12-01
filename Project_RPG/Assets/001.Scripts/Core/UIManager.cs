using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Rendering;

/// <summary>
/// UI 관리자 클래스 - 팝업, 씬 UI, 토스트 메시지 등을 관리
/// </summary>
public class UIManager
{
    // 팝업 UI의 Canvas 정렬 순서 (기본값: 10)
    private int order = 10;
    // 토스트 메시지의 Canvas 정렬 순서 (기본값: 500)
    private int toastOrder = 500;

    // 현재 활성화된 씬 UI를 반환하는 프로퍼티
    public UIScene SceneUI { get { return sceneUI; } }
    // 현재 활성화된 팝업들을 UIType으로 관리하는 딕셔너리
    public Dictionary<Define.UIType, UIPopup> activePopups = new Dictionary<Define.UIType, UIPopup>();

    // 팝업 UI를 스택 구조로 관리 (가장 최근에 열린 팝업이 맨 위)
    private Stack<UIPopup> popupStack = new Stack<UIPopup>();
    // 토스트 메시지를 큐 구조로 관리 (FIFO)
    private Queue<UIToast> toastQueue = new Queue<UIToast>();

    // UI 이벤트 처리를 위한 EventSystem 참조
    private EventSystem eventSystem = null;
    // 현재 활성화된 씬 UI 참조
    private UIScene sceneUI = null;

    // 화면 전환 시 사용되는 검은색 패널 (페이드 효과용)
    private CanvasGroup blackPanel;

    /// <summary>
    /// 검은색 패널을 반환하는 프로퍼티 (없으면 생성)
    /// </summary>
    public CanvasGroup BlackPanel
    {
        get
        {
            if (blackPanel == null)
            {
                SetEventSystem();
                GameObject go = GameObject.Find("@BlackPanel");
                if (go == null)
                {
                    go = Managers.Resource.Instantiate("@BlackPanel");
                    go.name = "@BlackPanel";
                    UnityEngine.Object.DontDestroyOnLoad(go);
                    blackPanel = go.GetOrAddComponent<CanvasGroup>();
                }
            }
            return blackPanel;
        }
    }

    /// <summary>
    /// 모든 UI의 루트 오브젝트를 반환하는 프로퍼티 (없으면 생성)
    /// </summary>
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    /// <summary>
    /// EventSystem을 설정하는 함수 (없으면 생성)
    /// </summary>
    public void SetEventSystem()
    {
        if (eventSystem != null) return;

        GameObject go = GameObject.Find("EventSystem");
        if (go == null)
            go = Managers.Resource.Instantiate("EventSystem");
        eventSystem = go.GetOrAddComponent<EventSystem>();
    }

    /// <summary>
    /// GameObject에 Canvas 설정을 추가하는 함수
    /// </summary>
    /// <param name="_go">Canvas를 설정할 GameObject</param>
    /// <param name="_sort">자동 정렬 여부 (기본값: true)</param>
    /// <param name="_sortOrder">수동 정렬 순서 (기본값: 0)</param>
    /// <param name="_isToast">토스트 메시지 여부 (기본값: false)</param>
    public void SetCanvas(GameObject _go, bool _sort = true, int _sortOrder = 0, bool _isToast = false)
    {
        GameObject go = GameObject.Find("EventSystem");
        if (go == null) SetEventSystem();
        Canvas canvas = _go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Camera.main;
        canvas.overrideSorting = true;

        CanvasScaler cs = _go.GetOrAddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);

        _go.GetOrAddComponent<GraphicRaycaster>();

        if (_sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = _sortOrder;
        }
        if (_isToast)
        {
            toastOrder++;
            canvas.sortingOrder = toastOrder;
        }
    }

    /// <summary>
    /// 씬 UI를 표시하는 함수
    /// </summary>
    /// <typeparam name="T">표시할 씬 UI 타입 (UIScene을 상속받아야 함)</typeparam>
    /// <param name="_name">씬 UI 프리팹 이름 (null이면 타입 이름 사용)</param>
    /// <returns>생성된 씬 UI 인스턴스</returns>
    public T ShowSceneUI<T>(string _name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(_name))
        {
            _name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"{_name}");

        T _sceneUI = go.GetOrAddComponent<T>();
        sceneUI = _sceneUI;

        go.transform.SetParent(Root.transform);

        return _sceneUI;
    }

    /// <summary>
    /// 씬 UI를 정리하는 함수
    /// </summary>
    /// <param name="_scene">정리할 씬 UI (현재는 사용하지 않음)</param>
    public void ClearScene(UIScene _scene)
    {
        sceneUI = null;
    }

    /// <summary>
    /// 팝업 UI를 표시하는 함수
    /// </summary>
    /// <typeparam name="T">표시할 팝업 UI 타입 (UIPopup을 상속받아야 함)</typeparam>
    /// <param name="_name">팝업 UI 프리팹 이름 (null이면 타입 이름 사용)</param>
    /// <param name="_pooling">오브젝트 풀링 사용 여부 (기본값: false)</param>
    /// <returns>생성된 팝업 UI 인스턴스</returns>
    public T ShowPopupUI<T>(string _name = null, bool _pooling = false) where T : UIPopup
    {
        if (string.IsNullOrEmpty(_name))
        {
            _name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"{_name}", _pooling: _pooling);
        if (_pooling) Managers.Pool.CreatePool(go);
        T popup = go.GetOrAddComponent<T>();
        popupStack.Push(popup);
        activePopups.Add(Util.ParseEnum<Define.UIType>(_name), popup);
        go.transform.SetParent(Root.transform);
        return popup;
    }

    /// <summary>
    /// 특정 팝업 UI를 닫는 함수 (스택의 최상단 팝업만 닫을 수 있음)
    /// </summary>
    /// <param name="_popup">닫을 팝업 UI</param>
    public void ClosePopupUI(UIPopup _popup)
    {
        if (popupStack.Count == 0)
            return;

        if (popupStack.Peek() != _popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }

    /// <summary>
    /// 스택의 최상단 팝업 UI를 닫는 내부 함수
    /// </summary>
    private void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UIPopup popup = popupStack.Pop();
        if (activePopups.ContainsKey(Util.ParseEnum<Define.UIType>(popup.GetType().Name)))
            activePopups.Remove(Util.ParseEnum<Define.UIType>(popup.GetType().Name));
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        order--;
    }

    /// <summary>
    /// 모든 팝업 UI를 닫는 함수
    /// </summary>
    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    /// <summary>
    /// 토스트 메시지를 표시하는 함수
    /// </summary>
    /// <param name="_description">표시할 메시지 내용</param>
    /// <returns>생성된 토스트 메시지 인스턴스</returns>
    public UIToast ShowToast(string _description)
    {
        string name = nameof(UIToast);
        GameObject go = Managers.Resource.Instantiate($"{name}", _pooling: true);
        UIToast popup = go.GetOrAddComponent<UIToast>();
        popup.SetInfo(_description);
        toastQueue.Enqueue(popup);
        go.transform.SetParent(Root.transform);
        return popup;
    }

    /// <summary>
    /// 큐의 첫 번째 토스트 메시지를 닫는 함수
    /// </summary>
    public void CloseToastUI()
    {
        if (toastQueue.Count == 0)
        {
            return;
        }

        UIToast toast = toastQueue.Dequeue();
        toast.Refresh();
        Managers.Resource.Destroy(toast.gameObject);
        toast = null;
        toastOrder--;
    }

    /// <summary>
    /// 모든 토스트 메시지를 닫는 함수
    /// </summary>
    public void CloseAllToastUI()
    {
        while (toastQueue.Count > 0)
        {
            Managers.Resource.Destroy(toastQueue.Dequeue().gameObject);
        }
    }

    /// <summary>
    /// 현재 열려있는 팝업의 개수를 반환하는 함수
    /// </summary>
    /// <returns>팝업 개수</returns>
    public int GetPopupCount()
    {
        return popupStack.Count;
    }

    /// <summary>
    /// 현재 활성화된 씬 UI를 특정 타입으로 반환하는 함수
    /// </summary>
    /// <typeparam name="T">반환할 씬 UI 타입 (UIScene을 상속받아야 함)</typeparam>
    /// <returns>씬 UI 인스턴스 (없으면 null)</returns>
    public T GetSceneUI<T>() where T : UIScene
    {
        return sceneUI as T;
    }

    /// <summary>
    /// UI 매니저를 초기화하는 함수 (모든 팝업 닫기, 씬 UI 초기화)
    /// </summary>
    public void Clear()
    {
        CloseAllPopupUI();

        Time.timeScale = 1;
        sceneUI = null;
    }
}
