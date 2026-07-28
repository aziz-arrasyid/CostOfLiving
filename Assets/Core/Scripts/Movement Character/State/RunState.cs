using Player.Animation;

namespace Player.StateMachine
{
    public class RunState : IPlayerState
    {
        private readonly PlayerStateMachine _sm;

        public RunState(PlayerStateMachine stateMachine)
        {
            _sm = stateMachine;
        }

        public void Enter()
        {
            _sm.Animator?.SetState(PlayerAnimState.Run);
            _sm.Audio?.PlayRunFootsteps();
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

            if (!motor.IsRunning)
            {
                _sm.ChangeState(_sm.Walk);
            }
        }

        public void Exit() { }
    }
}
