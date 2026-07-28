using Player.Animation;
using Player.Movement;

namespace Player.StateMachine
{
    public class FallState : IPlayerState
    {
        private readonly PlayerStateMachine _sm;

        public FallState(PlayerStateMachine stateMachine)
        {
            _sm = stateMachine;
        }

        public void Enter()
        {
            _sm.Animator?.SetState(PlayerAnimState.Fall);
            _sm.Audio?.StopFootsteps();
        }

        public void Update()
        {
            var motor = _sm.Motor;

            if (motor.IsGrounded)
            {
                _sm.ChangeState(GetGroundedNextState(motor));
            }
        }

        public void Exit()
        {
        }

        private IPlayerState GetGroundedNextState(PlayerMotor motor)
        {
            if (!motor.HasMoveInput) return _sm.Idle;
            return motor.IsRunning ? _sm.Run : _sm.Walk;
        }
    }
}
