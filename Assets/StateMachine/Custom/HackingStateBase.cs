using Deirin.StateMachine;

public abstract class HackingStateBase : StateBase
{
    HackingContext context;

    public override void Setup(IContext _context)
    {
        context = _context as HackingContext;
    }
}