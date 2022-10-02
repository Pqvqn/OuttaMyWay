using System.Collections.Generic;

public static class ActionKnowledge
{
    private static List<IAction> actions;

    public static void Clear()
    {
        actions = new List<IAction>() { new DefaultShove(), new DefaultGrab() };
    }
    
    public static IAction FindAction(ActionContext context)
    {
        IAction chosenAction = null;
        int complexity = -1;
        foreach (IAction action in actions)
        {
            if (action.Complexity > complexity && action.CanFire(context))
            {
                complexity = action.Complexity;
                chosenAction = action;
            }
        }
        return chosenAction;
    }

    public static List<IAction> GetPossibleUpgrades()
    {
        return null;
    }
}
