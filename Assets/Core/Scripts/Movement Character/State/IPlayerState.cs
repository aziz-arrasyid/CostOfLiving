namespace Player.StateMachine
{
    public interface IPlayerState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
