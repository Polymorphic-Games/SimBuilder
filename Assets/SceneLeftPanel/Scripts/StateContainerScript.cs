//using OpenCover.Framework.Model;
using SceneLeftPanel;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static ICommand;

namespace SceneLeftPanel
{
    public class StateContainerScript : MediatorComponent, IUpdateModelComponentNameHandler, IUpdateModelComponentValueHandler
    {
        public Model Model;
        public State State;

        public void OnDestroy()
        {
            State.UpdateModelComponentNameEventListener(this, false);
            State.UpdateModelComponentValueEventListener(this, false);
        }

        public void InitializeComponent(MediatorComponent mediator, Model model, State state)
        {
            base.InitializeComponent(mediator);
            Model = model;
            State = state;

            GetComponentInChildren<StateContainerColorScript>().setColor(state.color);
            GetComponentInChildren<StateInputFieldNameScript>().InitializeComponent(this, state);
            GetComponentInChildren<StateInputFieldValueScript>().InitializeComponent(this, state);
            GetComponentInChildren<RemoveButtonScript>().InitializeComponent(this);

            State.UpdateModelComponentNameEventListener(this);
            State.UpdateModelComponentValueEventListener(this);
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case RemoveButtonScript:
                    commandStack.Push(new CommandRemoveState(Model, State));
                    commandStack.Peek().Execute();
                    break;
                case StateInputFieldNameScript:
                    OnNotificationStateInputFieldNameScript(sender as StateInputFieldNameScript, args[0] as string);
                    //string text = (string)args[0];
                    //commandStack.Push(new CommandUpdateModelComponentName(State, text));
                    //commandStack.Peek().Execute();
                    break;
                case StateInputFieldValueScript:
                    string inputFieldText = ((StateInputFieldValueScript)sender).Text;
                    string newInputFieldText = (string)args[0];
                    float value;
                    if (float.TryParse(newInputFieldText, out value))
                    {
                        commandStack.Push(new CommandUpdateModelComponentValue(State, value));
                        commandStack.Peek().Execute();
                    }
                    else
                    {
                        GetComponentInChildren<StateInputFieldValueScript>().SetTextWithoutNotify(State.Value.ToString());
                    }
                    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
        }

        void OnNotificationStateInputFieldNameScript(StateInputFieldNameScript sender, string newName)
        {
            if (newName.Length > 1 || !Regex.IsMatch(newName, @"^[a-zA-z]+$"))
            {
                sender.SetTextWithoutNotify(sender.Text);
                return;
            }
            newName = newName.ToUpper();
            foreach (var state in Model.StateDictionary.Values)
            {
                if (newName == state.Name)
                {
                    sender.SetTextWithoutNotify(sender.Text);
                    return;
                }
            }

            commandStack.Push(new CommandUpdateModelComponentName(State, newName.ToUpper()));
            commandStack.Peek().Execute();
        }

        public void UpdateModelComponentNameCallback(object sender, string name)
        {
            GetComponentInChildren<StateInputFieldNameScript>().SetTextWithoutNotify(name);
        }

        public void UpdateModelComponentValueCallback(object sender, float value)
        {
            GetComponentInChildren<StateInputFieldValueScript>().SetTextWithoutNotify(value.ToString());
        }

    }
}
