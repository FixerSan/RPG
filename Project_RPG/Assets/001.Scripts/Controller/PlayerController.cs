using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    private Rigidbody rb;
    public Joystick joystick;
    public float moveSpeed = 5f;    
    public float rotationSpeed = 10f;

    public Dictionary<Define.PlayerState, State<PlayerController>> states;
    public StateMachine<PlayerController> sm;
    public Define.PlayerState CurrentState;
    private Define.PlayerState currentState;



    public void Init()
    {
        anim = Util.FindChild<Animator>(gameObject, "Model");
        rb = gameObject.GetOrAddComponent<Rigidbody>();
        states = states = new Dictionary<Define.PlayerState, State<PlayerController>>();
        states.Add(Define.PlayerState.IDLE, new PlayerState.Idle());
        states.Add(Define.PlayerState.MOVE, new PlayerState.Move());
        sm = new StateMachine<PlayerController>(this, states[Define.PlayerState.IDLE]);
    }

    public void ChangeState(Define.PlayerState _state, bool changeSameState = false)
    {
        if (_state == CurrentState && !changeSameState) return;

        sm.ChangeState(states[_state]);
        currentState = _state;
        CurrentState = _state;
    }

    void Awake()
    {
        Init();
    }

    void FixedUpdate()
    {
        CheckMoveAndStop();
        sm.FixedUpdate();
    }

    private void Update()
    {
        sm.Update();
    }

    public void CheckMoveAndStop()
    {
        Vector2 input = joystick.Direction;
        if (input == Vector2.zero) ChangeState(Define.PlayerState.IDLE);
        else ChangeState(Define.PlayerState.MOVE);
    }

    public void Move()
    {
        Vector3 moveDir = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);

        // 회전 (캐릭터가 조이스틱 방향을 보게 함)
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 이동
        rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.deltaTime);
    }
}
