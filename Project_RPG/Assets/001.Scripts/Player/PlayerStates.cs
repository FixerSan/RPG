using UnityEngine;

namespace PlayerStates
{

    public class Idle : State<PlayerController>
    {

        public override void Enter(PlayerController _entity)
        {
            
        }

        public override void Update(PlayerController _entity)
        {
            _entity.Player.move.CheckMove();
        }

        public override void FixedUpdate(PlayerController _entity)
        {

        }

        public override void Exit(PlayerController _entity)
        {

        }

    }

    public class Move : State<PlayerController>
    {

        public override void Enter(PlayerController _entity)
        {
            _entity.anim.SetBool("IsMove", true);
        }

        public override void Update(PlayerController _entity)
        {
            _entity.Player.move.CheckStop();
        }

        public override void FixedUpdate(PlayerController _entity)
        {
            _entity.Player.move.Move();
        }

        public override void Exit(PlayerController _entity)
        {
            _entity.anim.SetBool("IsMove", false);
        }

    }
    public class Attack : State<PlayerController>
    {

        public override void Enter(PlayerController _entity)
        {

        }

        public override void Update(PlayerController _entity)
        {

        }

        public override void FixedUpdate(PlayerController _entity)
        {

        }

        public override void Exit(PlayerController _entity)
        {

        }

    }


    public class Hit : State<PlayerController>
    {

        public override void Enter(PlayerController _entity)
        {

        }

        public override void Update(PlayerController _entity)
        {

        }

        public override void FixedUpdate(PlayerController _entity)
        {

        }

        public override void Exit(PlayerController _entity)
        {

        }

    }

    public class Death : State<PlayerController>
    {

        public override void Enter(PlayerController _entity)
        {

        }

        public override void Update(PlayerController _entity)
        {

        }

        public override void FixedUpdate(PlayerController _entity)
        {

        }

        public override void Exit(PlayerController _entity)
        {

        }

    }

}


