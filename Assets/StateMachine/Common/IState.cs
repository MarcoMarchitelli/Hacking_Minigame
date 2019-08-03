namespace Deirin.StateMachine
{
    public interface IState
    {
        void Setup(IContext _context);

        /// <summary>
        /// Called once on state enter.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called each frame while state is active.
        /// </summary>
        void Tick();

        /// <summary>
        /// Called once on state exit.
        /// </summary>
        void Exit();
    } 
}