using SceneLeftPanel;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

namespace SceneLeftPanel
{
    public class ConnectionContainerScript : MediatorComponent, IUpdateConnectionToStateHandler, IUpdateConnectionFromStateHandler, IUpdateModelComponentNameHandler
    {
        Model Model;
        Connection Connection;

        public void OnDestroy()
        {
            Connection.UpdateToStateEventListener(this, false);
            Connection.ToState?.UpdateModelComponentNameEventListener(this, false);

            Connection.UpdateFromStateEventListener(this, false);
            Connection.FromState?.UpdateModelComponentNameEventListener(this, false);
        }

        public void InitializeComponent(MediatorComponent mediator, Model model, Connection connection)
        {
            base.InitializeComponent(mediator);
            Model = model;
            Connection = connection;
            GetComponentInChildren<ConnectionContainerFromStateDropdown>().InitializeComponent(this, Model, Connection.FromState);
            GetComponentInChildren<ConnectionContainerToStateDropdown>().InitializeComponent(this, Model, Connection.ToState);
            //GetComponentInChildren<ConnectionContainerFromStateInputField>().InitializeComponent(this, Connection.FromState);
            //GetComponentInChildren<ConnectionContainerToStateInputField>().InitializeComponent(this, Connection.ToState);
            GetComponentInChildren<RateEquationContainerScript>().InitializeComponent(this, Model, Connection.RateEquation);
            GetComponentInChildren<RemoveButtonScript>().InitializeComponent(this);

            Connection.UpdateToStateEventListener(this);
            Connection.UpdateFromStateEventListener(this);
            Connection.FromState?.UpdateModelComponentNameEventListener(this);
            Connection.ToState?.UpdateModelComponentNameEventListener(this);
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case RemoveButtonScript:
                    commandStack.Push(new CommandRemoveConnection(Model, Connection));
                    commandStack.Peek().Execute();
                    break;
                case ConnectionContainerFromStateDropdown:
                    OnNotificationConnectionContainerFromStateDropdown(args[0] as State);
                    break;
                case ConnectionContainerToStateDropdown:
                    OnNotificationConnectionContainerToStateDropdown(args[0] as State);
                    break;
                //case ConnectionContainerToStateInputField:
                //    OnNotificationConnectionContainerToStateInputField(sender, args);
                //    break;
                //case ConnectionContainerFromStateInputField:
                //    OnNotificationConnectionContainerFromStateInputField(sender, args);
                //    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
        }

        void OnNotificationConnectionContainerFromStateDropdown(State newFromState)
        {
            if (newFromState != null)
            {
                commandStack.Push(new CommandUpdateFromState(Connection, newFromState));
                commandStack.Peek().Execute();
            }

        }

        void OnNotificationConnectionContainerToStateDropdown(State newToState)
        {
            if (newToState != null)
            {
                commandStack.Push(new CommandUpdateToState(Connection, newToState));
                commandStack.Peek().Execute();
            }

        }

        //void OnNotificationConnectionContainerFromStateInputField(object sender, params object[] args)
        //{
        //    ConnectionContainerFromStateInputField Sender = (ConnectionContainerFromStateInputField)sender;
        //    string StateName = (string)args[0];
        //    StateName = StateName.ToUpper();
        //    State newFromState = (State)Model.GetDictionaryValue(StateName, Model.StateDictionary);
        //    if (newFromState != null)
        //    {
        //        commandStack.Push(new CommandUpdateFromState(Connection, newFromState));
        //        commandStack.Peek().Execute();
        //    }
        //    else
        //    {
        //        GetComponentInChildren<ConnectionContainerFromStateInputField>().resetText();
        //    }
        //}

        //void OnNotificationConnectionContainerToStateInputField(object sender, params object[] args)
        //{
        //    //ConnectionContainerToStateInputField Sender = (ConnectionContainerToStateInputField)sender;
        //    string stateName = (string)args[0];
        //    State state;

        //    if (stateName == "")
        //    {
        //        state = null;
        //    }
        //    else
        //    {
        //        stateName = stateName.ToUpper();
        //        state = (State)Model.GetDictionaryValue(stateName, Model.StateDictionary);
        //        if (state == null) { return; }
        //    }

        //    //State newToState = (State)Model.GetDictionaryValue(stateName, Model.StateDictionary);
        //    //if (newToState != null)
        //    //{
        //        commandStack.Push(new CommandUpdateToState(Connection, state));
        //        commandStack.Peek().Execute();
        //    //}
        //    //else
        //    //{
        //        //GetComponentInChildren<ConnectionContainerToStateInputField>().resetText();
        //    //}
        //}

        public void UpdateToStateCallback(object sender, State newState)
        {
            if (newState == null)
            {
                GetComponentInChildren<ConnectionContainerToStateInputField>().SetTextWithoutNotify("");
            }
            else
            {
                Connection connection = (Connection)sender;
                connection.ToState?.UpdateModelComponentNameEventListener(this, false);
                newState?.UpdateModelComponentNameEventListener(this);
                //GetComponentInChildren<ConnectionContainerToStateInputField>().SetTextWithoutNotify(newState?.Name);
            }
        }       
     

        public void UpdateFromStateCallback(object sender, State newState)
        {
            if (newState == null)
            {
                GetComponentInChildren<ConnectionContainerFromStateInputField>().resetText();
                return;
            }

            Connection connection = (Connection) sender;
            connection.FromState?.UpdateModelComponentNameEventListener(this, false);
            newState?.UpdateModelComponentNameEventListener(this);
            //GetComponentInChildren<ConnectionContainerFromStateInputField>().SetTextWithoutNotify(newState?.Name);
            GetComponentInChildren<RateEquationFromStateScript>().setValue(newState?.Name);
        }

        public void UpdateModelComponentNameCallback(object sender, string stateName)
        {
            State state = (State)sender;
            if (Connection.FromState == state)
            {
                GetComponentInChildren<RateEquationFromStateScript>().setValue(stateName);
            }
           
        }

    }
}
