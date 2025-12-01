using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    public Player player;
    public Animator anim;
    public Rigidbody rb;
    public float rotationSpeed = 10f;

    public Dictionary<Define.PlayerState, State<PlayerController>> states;
    public StateMachine<PlayerController> sm;
    public Define.PlayerState CurrentState;
    private Define.PlayerState currentState;



    public void Init()
    {
        player = new Player(this ,100, 100, 5f);
        anim = Util.FindChild<Animator>(gameObject, "Model");
        rb = gameObject.GetOrAddComponent<Rigidbody>();
        states = states = new Dictionary<Define.PlayerState, State<PlayerController>>();
        states.Add(Define.PlayerState.IDLE, new PlayerStates.Idle());
        states.Add(Define.PlayerState.MOVE, new PlayerStates.Move());
        states.Add(Define.PlayerState.ATTACK, new PlayerStates.Attack());
        states.Add(Define.PlayerState.HIT, new PlayerStates.Hit());
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
        sm.FixedUpdate();
    }

    private void Update()
    {
        sm.Update();
    }
}
