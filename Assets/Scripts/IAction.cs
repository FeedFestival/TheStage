using System.Collections.Generic;

public interface IAction
{
    void InitAction(FinishEvent onFinish);
    void Do(IAct act = null);

    void ContinueAction();
}

public enum Sequence
{
    Animation,
    Sound,
    Line
}
