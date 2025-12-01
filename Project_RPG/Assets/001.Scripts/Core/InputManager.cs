using UnityEngine;

public class InputManager
{
    private Joystick joystick;
    public bool IsMoveInput { get { return joystick.Direction.magnitude > 0.01f; } }
    public Vector3 MoveDirection { get { return new Vector3(joystick.Direction.x, 0, joystick.Direction.y); } }

    public InputManager()
    {
        joystick = GameObject.Find("Joystick").GetOrAddComponent<Joystick>();
    }
}
