using UnityEngine;

namespace PlayerState 
{
    public class Idle : State<PlayerController>
    {
        public override void Enter(PlayerController _entity)
        {

        }

        public override void Exit(PlayerController _entity)
        {

        }

        public override void FixedUpdate(PlayerController _entity)
        {

        }

        public override void Update(PlayerController _entity)
        {

        }
    }

    public class Move : State<PlayerController>
    {
        public override void Enter(PlayerController _entity)
        {
            _entity.anim.SetBool("IsMove", true);
        }

        public override void Exit(PlayerController _entity)
        {
            _entity.anim.SetBool("IsMove", false);

        }

        public override void FixedUpdate(PlayerController _entity)
        {
            _entity.Move();
        }

        public override void Update(PlayerController _entity)
        {

        }
    }
}
