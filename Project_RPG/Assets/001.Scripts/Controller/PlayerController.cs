using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    private bool init = false;
    public Player Player => Managers.Game.player;
    public Animator anim;
    public Rigidbody rb;
    public float rotationSpeed = 10f;

    public Dictionary<Define.PlayerState, State<PlayerController>> states;
    public StateMachine<PlayerController> sm;
    public Define.PlayerState CurrentState;
    private Define.PlayerState currentState;

    public void Start()
    {
        Managers.Object.SetPlayerController(this);
        Managers.Data.SavePlayerData();
    }

    public void Init()
    {
        anim = Util.FindChild<Animator>(gameObject, "Model");
        rb = gameObject.GetOrAddComponent<Rigidbody>();
        states = states = new Dictionary<Define.PlayerState, State<PlayerController>>();
        states.Add(Define.PlayerState.IDLE, new PlayerStates.Idle());
        states.Add(Define.PlayerState.MOVE, new PlayerStates.Move());
        states.Add(Define.PlayerState.ATTACK, new PlayerStates.Attack());
        states.Add(Define.PlayerState.HIT, new PlayerStates.Hit());
        sm = new StateMachine<PlayerController>(this, states[Define.PlayerState.IDLE]);
        init = true;
    }

    public void ChangeState(Define.PlayerState _state, bool changeSameState = false)
    {
        if (_state == CurrentState && !changeSameState) return;

        sm.ChangeState(states[_state]);
        currentState = _state;
        CurrentState = _state;
    }

    void FixedUpdate()
    {
        if (!init) return;
        sm.FixedUpdate();
    }

    private void Update()
    {
        if (!init) return;
        sm.Update();
    }
}
