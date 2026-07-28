using Player.Animation;

namespace Player.StateMachine
{
    public class IdleState : IPlayerState
    {
        private readonly PlayerStateMachine _sm;

        public IdleState(PlayerStateMachine stateMachine)
        {
            _sm = stateMachine;
        }

        public void Enter()
        {
            _sm.Animator?.SetState(PlayerAnimState.Idle);
            _sm.Audio?.StopFootsteps();
        }

        public void Update()
        {
            var motor = _sm.Motor;

            if (!motor.IsGrounded)
            {
                _sm.ChangeState(motor.VerticalVelocity > 0f ? (IPlayerState)_sm.Jump : _sm.Fall);
                return;
            }

            if (motor.HasMoveInput)
            {
                _sm.ChangeState(motor.IsRunning ? (IPlayerState)_sm.Run : _sm.Walk);
            }
        }

        public void Exit() { }
    }
}
