using Deirin.StateMachine;

public class HackingStateMachine : StateMachineBase
{
    public HackingContext context;

    protected override void CustomSetup()
    {
        DontDestroyOnLoad(this);
    }

    protected override void ContextSetup()
    {
        context.GoForward += GoForward;
    }

    private void GoForward()
    {
        _data.SetTrigger("Forward");
    }
}

[System.Serializable]
public class HackingContext : IContext
{
    public ScreenFader screenFaderPrefab;

    public System.Action GoForward;
}