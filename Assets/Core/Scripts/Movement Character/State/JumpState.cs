using Player.Animation;
using Player.Movement;

namespace Player.StateMachine
{
    public class JumpState : IPlayerState
    {
        private readonly PlayerStateMachine _sm;

        public JumpState(PlayerStateMachine stateMachine)
        {
            _sm = stateMachine;
        }

        public void Enter()
        {
            // _sm.Animator?.SetState(PlayerAnimState.Jump);
            _sm.Audio?.StopFootsteps();
            _sm.Audio?.PlayJumpSound();
        }

        public void Update()
        {
            var motor = _sm.Motor;

            if (motor.IsGrounded)
            {
                _sm.ChangeState(GetGroundedNextState(motor));
                return;
            }

            if (motor.VerticalVelocity <= 0f)
            {
                _sm.ChangeState(_sm.Fall);
            }
        }

        public void Exit() { }

        private IPlayerState GetGroundedNextState(PlayerMotor motor)
        {
            if (!motor.HasMoveInput) return _sm.Idle;
            return motor.IsRunning ? _sm.Run : _sm.Walk;
        }
    }
}
