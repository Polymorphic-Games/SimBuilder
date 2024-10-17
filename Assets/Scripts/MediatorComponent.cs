using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MediatorComponent : MonoBehaviour
{
    public MediatorComponent mediator;
    public Stack<ICommand> commandStack = new();

    public virtual void InitializeComponent(MediatorComponent mediator)
    {
        this.mediator = mediator;
        commandStack = mediator.commandStack;
    }

    public virtual void Notify(object sender, params object[] args)
    {
        if (mediator != null)
        {
            mediator.Notify(sender, args);
        }
    }

}
