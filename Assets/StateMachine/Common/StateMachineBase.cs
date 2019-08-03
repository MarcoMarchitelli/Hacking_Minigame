namespace Deirin.StateMachine
{
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public abstract class StateMachineBase : MonoBehaviour, IStateMachine
    {
        [SerializeField] bool setupOnStart = false;

        protected Animator _data;

        public IContext CurrentContext { get; set; }
        public Animator Data
        {
            get
            {
                if (!_data)
                    _data = GetComponent<Animator>();

                return _data;
            }
        }

        private void Start()
        {
            if (setupOnStart)
                Setup();
        }

        public void Setup()
        {
            ContextSetup();
            foreach (StateBase smB in Data.GetBehaviours<StateBase>())
            {
                StateBase state = smB;
                if (state)
                    state.Setup(CurrentContext);
            }
            CustomSetup();
        }

        protected abstract void ContextSetup();

        protected virtual void CustomSetup()
        {

        }
    } 
}