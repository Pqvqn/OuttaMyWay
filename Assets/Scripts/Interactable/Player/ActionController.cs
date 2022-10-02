using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionController : MonoBehaviour {

    public ActionContext context = new ActionContext()
    {
        push = ButtonContext.Released,
        pull = ButtonContext.Released,
        currentAction = null
    };
    public InputAction push;
    public InputAction pull;

    private void OnEnable()
    {
        push.Enable();
        pull.Enable();
    }

    private void OnDisable()
    {
        push.Disable();
        pull.Disable();
    }


    private void AttemptFire()
    {
        IAction action = ActionKnowledge.FindAction(context);
        if(action != null)
        {
            action.Fire(context);
            context.currentAction = action;
        }
    }


    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        bool attemptFire = false;
        if (context.currentAction != null)
        {
            attemptFire = context.currentAction.FixedUpdate(context);
            if(attemptFire && context.currentAction.State == ActionState.Dead)
            {
                context.currentAction = null; 
            }
        }

        bool pushes = push.ReadValue<float>()  > 0.5f;
        bool pulls = pull.ReadValue<float>() > 0.5f;

        if(context.push == ButtonContext.Released && pushes)
        {
            context.push = ButtonContext.Pressing;
            attemptFire = true;
        }
        else if (context.push == ButtonContext.Pressed && !pushes)
        {
            context.push = ButtonContext.Releasing;
            attemptFire = true;
        }
        else if (context.push == ButtonContext.Pressing)
        {
            if (pushes)
            {
                context.push = ButtonContext.Pressed;
            }
            else
            {
                context.push = ButtonContext.Released;
            }
            attemptFire = true;
        }
        else if (context.push == ButtonContext.Releasing)
        {
            if (pushes)
            {
                context.push = ButtonContext.Pressed;
            }
            else
            {
                context.push = ButtonContext.Released;
            }
            attemptFire = true;
        }

        if (context.pull == ButtonContext.Released && pulls)
        {
            context.pull = ButtonContext.Pressing;
            attemptFire = true;
        }
        else if (context.pull == ButtonContext.Pressed && !pulls)
        {
            context.pull = ButtonContext.Releasing;
            attemptFire = true;
        }
        else if (context.pull == ButtonContext.Pressing)
        {
            if (pulls)
            {
                context.pull = ButtonContext.Pressed;
            }
            else
            {
                context.pull = ButtonContext.Released;
            }
            attemptFire = true;
        }
        else if (context.pull == ButtonContext.Releasing)
        {
            if (pulls)
            {
                context.pull = ButtonContext.Pressed;
            }
            else
            {
                context.pull = ButtonContext.Released;
            }
            attemptFire = true;
        }

        if (attemptFire)
        {
            AttemptFire();
        }
    }
}
