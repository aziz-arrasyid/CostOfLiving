using Player.Animation;
using Player.Audio;
using Player.Movement;
using UnityEngine;

namespace Player.StateMachine
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerStateMachine : MonoBehaviour
    {
        public PlayerMotor Motor { get; private set; }
        public IPlayerState CurrentState { get; private set; }

        public PlayerAnimator Animator { get; private set; }

        public IdleState Idle { get; private set; }
        public WalkState Walk { get; private set; }
        public RunState Run { get; private set; }
        public JumpState Jump { get; private set; }
        public FallState Fall { get; private set; }
        public PlayerAudio Audio { get; private set; }

        private void Awake()
        {
            Motor = GetComponent<PlayerMotor>();
            Animator = GetComponent<PlayerAnimator>();
            Audio = GetComponent<PlayerAudio>();


            Idle = new IdleState(this);
            Walk = new WalkState(this);
            Run = new RunState(this);
            Jump = new JumpState(this);
            Fall = new FallState(this);
        }

        private void Start()
        {
            ChangeState(Idle);
        }

        private void Update()
        {
            CurrentState?.Update();
        }

        public void ChangeState(IPlayerState newState)
        {
            if (CurrentState == newState) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}