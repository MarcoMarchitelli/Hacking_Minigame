namespace Deirin.StateMachine
{
    using UnityEngine;

    public abstract class StateBase : StateMachineBehaviour, IState
    {
        public abstract void Setup(IContext _context);

        public virtual void Enter()
        {

        }

        public virtual void Tick()
        {

        }

        public virtual void Exit()
        {

        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            Enter();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            Tick();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            Exit();
        }
    } 
}