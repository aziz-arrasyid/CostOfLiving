using Player.Animation;

namespace Player.StateMachine
{
    public class WalkState : IPlayerState
    {
        private readonly PlayerStateMachine _sm;

        public WalkState(PlayerStateMachine stateMachine)
        {
            _sm = stateMachine;
        }

        public void Enter()
        {
            _sm.Animator?.SetState(PlayerAnimState.Walk);
            _sm.Audio?.PlayWalkFootsteps();
        }

        public void Update()
        {
            var motor = _sm.Motor;

            if (!motor.IsGrounded)
            {
                _sm.ChangeState(motor.VerticalVelocity > 0f ? (IPlayerState)_sm.Jump : _sm.Fall);
                return;
            }

            if (!motor.HasMoveInput)
            {
                _sm.ChangeState(_sm.Idle);
                return;
            }

            if (motor.IsRunning)
            {
                _sm.ChangeState(_sm.Run);
            }
        }

        public void Exit() { }
    }
}
