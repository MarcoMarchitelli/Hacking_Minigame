namespace Deirin.StateMachine
{
    using UnityEngine;

    public interface IStateMachine
    {
        IContext CurrentContext { set; get; }
        Animator Data { get; }

        void Setup();
    } 
}