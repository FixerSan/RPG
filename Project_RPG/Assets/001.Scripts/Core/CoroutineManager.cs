using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 코루틴을 중앙에서 관리하는 매니저 클래스
/// MonoBehaviour가 없는 클래스에서도 코루틴을 사용할 수 있도록 함
/// </summary>
public class CoroutineManager : Singleton<CoroutineManager>
{
    // 실행 중인 코루틴을 추적하기 위한 딕셔너리
    private Dictionary<string, Coroutine> runningCoroutines = new Dictionary<string, Coroutine>();

    /// <summary>
    /// 코루틴을 시작하고 추적하는 함수
    /// </summary>
    /// <param name="coroutine">시작할 코루틴</param>
    /// <param name="key">코루틴을 식별할 키 (null이면 자동 생성)</param>
    /// <returns>시작된 코루틴</returns>
    public Coroutine StartCoroutine(IEnumerator coroutine, string key = null)
    {
        if (coroutine == null)
        {
            Debug.LogWarning("CoroutineManager: 코루틴이 null입니다.");
            return null;
        }

        if (string.IsNullOrEmpty(key))
        {
            key = Guid.NewGuid().ToString();
        }

        // 이미 같은 키로 실행 중인 코루틴이 있으면 중지
        if (runningCoroutines.ContainsKey(key))
        {
            StopCoroutine(key);
        }

        Coroutine startedCoroutine = base.StartCoroutine(CoroutineWrapper(coroutine, key));
        runningCoroutines[key] = startedCoroutine;

        return startedCoroutine;
    }

    /// <summary>
    /// 특정 키로 실행 중인 코루틴을 중지하는 함수
    /// </summary>
    /// <param name="key">중지할 코루틴의 키</param>
    public new void StopCoroutine(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("CoroutineManager: 키가 null이거나 비어있습니다.");
            return;
        }

        if (runningCoroutines.TryGetValue(key, out Coroutine coroutine))
        {
            if (coroutine != null)
            {
                base.StopCoroutine(coroutine);
            }
            runningCoroutines.Remove(key);
        }
    }

    /// <summary>
    /// 모든 실행 중인 코루틴을 중지하는 함수
    /// </summary>
    public new void StopAllCoroutines()
    {
        base.StopAllCoroutines();
        runningCoroutines.Clear();
    }

    /// <summary>
    /// 특정 키로 실행 중인 코루틴이 있는지 확인하는 함수
    /// </summary>
    /// <param name="key">확인할 키</param>
    /// <returns>실행 중이면 true, 아니면 false</returns>
    public bool IsCoroutineRunning(string key)
    {
        return runningCoroutines.ContainsKey(key) && runningCoroutines[key] != null;
    }

    /// <summary>
    /// 코루틴을 래핑하여 완료 시 딕셔너리에서 제거하는 함수
    /// </summary>
    private IEnumerator CoroutineWrapper(IEnumerator coroutine, string key)
    {
        yield return coroutine;
        if (runningCoroutines.ContainsKey(key))
        {
            runningCoroutines.Remove(key);
        }
    }

    #region 유틸리티 함수들

    /// <summary>
    /// 지정된 시간만큼 대기하는 코루틴
    /// </summary>
    /// <param name="seconds">대기할 시간 (초)</param>
    /// <param name="onComplete">완료 시 호출할 콜백</param>
    /// <param name="key">코루틴 키 (null이면 자동 생성)</param>
    public void WaitForSeconds(float seconds, Action onComplete = null, string key = null)
    {
        StartCoroutine(WaitForSecondsCoroutine(seconds, onComplete), key);
    }

    private IEnumerator WaitForSecondsCoroutine(float seconds, Action onComplete)
    {
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }

    /// <summary>
    /// 다음 프레임까지 대기하는 코루틴
    /// </summary>
    /// <param name="onComplete">완료 시 호출할 콜백</param>
    /// <param name="key">코루틴 키 (null이면 자동 생성)</param>
    public void WaitForNextFrame(Action onComplete = null, string key = null)
    {
        StartCoroutine(WaitForNextFrameCoroutine(onComplete), key);
    }

    private IEnumerator WaitForNextFrameCoroutine(Action onComplete)
    {
        yield return null;
        onComplete?.Invoke();
    }

    /// <summary>
    /// 조건이 만족될 때까지 대기하는 코루틴
    /// </summary>
    /// <param name="condition">만족할 조건 (true가 되면 완료)</param>
    /// <param name="onComplete">완료 시 호출할 콜백</param>
    /// <param name="key">코루틴 키 (null이면 자동 생성)</param>
    public void WaitUntil(Func<bool> condition, Action onComplete = null, string key = null)
    {
        if (condition == null)
        {
            Debug.LogWarning("CoroutineManager: 조건이 null입니다.");
            return;
        }
        StartCoroutine(WaitUntilCoroutine(condition, onComplete), key);
    }

    private IEnumerator WaitUntilCoroutine(Func<bool> condition, Action onComplete)
    {
        yield return new WaitUntil(condition);
        onComplete?.Invoke();
    }

    /// <summary>
    /// 지정된 시간 동안 반복 실행하는 코루틴
    /// </summary>
    /// <param name="duration">반복할 시간 (초)</param>
    /// <param name="onUpdate">매 프레임 호출할 콜백 (경과 시간을 매개변수로 전달)</param>
    /// <param name="onComplete">완료 시 호출할 콜백</param>
    /// <param name="key">코루틴 키 (null이면 자동 생성)</param>
    public void DoForSeconds(float duration, Action<float> onUpdate = null, Action onComplete = null, string key = null)
    {
        StartCoroutine(DoForSecondsCoroutine(duration, onUpdate, onComplete), key);
    }

    private IEnumerator DoForSecondsCoroutine(float duration, Action<float> onUpdate, Action onComplete)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            onUpdate?.Invoke(elapsed);
            yield return null;
        }
        onComplete?.Invoke();
    }

    /// <summary>
    /// 지정된 횟수만큼 반복 실행하는 코루틴
    /// </summary>
    /// <param name="count">반복 횟수</param>
    /// <param name="onUpdate">매 반복마다 호출할 콜백 (현재 반복 횟수를 매개변수로 전달)</param>
    /// <param name="onComplete">완료 시 호출할 콜백</param>
    /// <param name="key">코루틴 키 (null이면 자동 생성)</param>
    public void DoForCount(int count, Action<int> onUpdate = null, Action onComplete = null, string key = null)
    {
        StartCoroutine(DoForCountCoroutine(count, onUpdate, onComplete), key);
    }

    private IEnumerator DoForCountCoroutine(int count, Action<int> onUpdate, Action onComplete)
    {
        for (int i = 0; i < count; i++)
        {
            onUpdate?.Invoke(i);
            yield return null;
        }
        onComplete?.Invoke();
    }

    /// <summary>
    /// 지정된 시간 간격으로 반복 실행하는 코루틴
    /// </summary>
    /// <param name="interval">반복 간격 (초)</param>
    /// <param name="onUpdate">매 반복마다 호출할 콜백</param>
    /// <param name="onComplete">완료 시 호출할 콜백</param>
    /// <param name="key">코루틴 키 (null이면 자동 생성)</param>
    public void DoInterval(float interval, Action onUpdate = null, Action onComplete = null, string key = null)
    {
        StartCoroutine(DoIntervalCoroutine(interval, onUpdate, onComplete), key);
    }

    private IEnumerator DoIntervalCoroutine(float interval, Action onUpdate, Action onComplete)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            onUpdate?.Invoke();
        }
    }

    /// <summary>
    /// 지정된 시간 간격으로 반복 실행하는 코루틴 (최대 반복 횟수 제한)
    /// </summary>
    /// <param name="interval">반복 간격 (초)</param>
    /// <param name="maxCount">최대 반복 횟수</param>
    /// <param name="onUpdate">매 반복마다 호출할 콜백 (현재 반복 횟수를 매개변수로 전달)</param>
    /// <param name="onComplete">완료 시 호출할 콜백</param>
    /// <param name="key">코루틴 키 (null이면 자동 생성)</param>
    public void DoIntervalWithCount(float interval, int maxCount, Action<int> onUpdate = null, Action onComplete = null, string key = null)
    {
        StartCoroutine(DoIntervalWithCountCoroutine(interval, maxCount, onUpdate, onComplete), key);
    }

    private IEnumerator DoIntervalWithCountCoroutine(float interval, int maxCount, Action<int> onUpdate, Action onComplete)
    {
        for (int i = 0; i < maxCount; i++)
        {
            yield return new WaitForSeconds(interval);
            onUpdate?.Invoke(i);
        }
        onComplete?.Invoke();
    }

    #endregion
}
