public class StateMachine<T> where T : class
{
    // 상태머신이 관리할 엔티티
    public T entity;

    // 현재 상태를 참조
    public State<T> currentState;

    // 상태머신이 초기화 되었는지 여부를 저장
    private bool isInit = false;

    // 상태 전환이 진행중임을 나타냄
    public bool isChange = false;

    // 상태머신 생성자, 엔티티와 첫 상태를 받아서 상태 전이 및 초기화
    public StateMachine(T _entity, State<T> _firstState)
    {
        entity = _entity;
        ChangeState(_firstState);
        isInit = true;
    }

    // 상태 전이 함수, 현재 상태가 있을 경우 Exit 호출 후 새로운 상태로 전환 및 Enter 호출
    public void ChangeState(State<T> _changeState)
    {
        isChange = true;
        if (currentState != null)
            currentState.Exit(entity);
        currentState = _changeState;
        currentState.Enter(entity);
        isChange = false;
    }

    // 매 프레임마다 호출, 상태가 초기화 및 전이 중이 아닐 때 현재 상태의 Update 호출
    public void Update()
    {
        if (!isInit) return;
        if (isChange) return;
        currentState.Update(entity);
    }

    // 고정 업데이트(물리 연산 등)에 호출, 상태가 초기화 및 전이 중이 아닐 때 현재 상태의 FixedUpdate 호출
    public void FixedUpdate()
    {
        if (!isInit) return;
        if (isChange) return;
        currentState.FixedUpdate(entity);
    }
}
