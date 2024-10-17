using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SceneLeftPanel
{
    public class ConnectionContainerToStateDropdown : DropdownScript, IStateHandler, IUpdateModelComponentNameHandler
    {
        Model Model;
        List<State> States = new();
        List<string> DropdownOptions = new();
        Dictionary<int, ModelComponent> DropDownValueToModelComponent = new();
        Dictionary<int, int> ModelComponentUniqueIDtoDropDownValue = new();

        void OnDestroy()
        {
            Model.AddStateEventListener(this, false);

            foreach (var state in Model.StateDictionary.Values)
            {
                state.UpdateModelComponentNameEventListener(this, false);
            }
        }

        public void InitializeComponent(MediatorComponent mediator, Model model, State toState)
        {
            base.InitializeComponent(mediator);
            Model = model;

            AddModelStatesToDropdown();
            Model.AddStateEventListener(this);
            RebuildDropdownOptions();

            if (toState != null)
            {
                int initialStateIndex = ModelComponentUniqueIDtoDropDownValue[toState.UniqueID];
                SetValueWithoutNotify(initialStateIndex);
            }
        }

        #region Notifications
        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case DropdownScript:
                    OnNotificationDropDownScript(sender as DropdownScript, args);
                    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
        }

        void OnNotificationDropDownScript(DropdownScript sender, params object[] args)
        {
            int dropdownValue = (int)args[0];
            ModelComponent modelComponent;

            if (dropdownValue == 0)
            {
                modelComponent = null;
            }
            else
            {
                modelComponent = DropDownValueToModelComponent[dropdownValue];
            }
            mediator.Notify(this, modelComponent as State);
        }
        #endregion


        #region Callbacks

        public void AddStateCallback(object sender, State newState)
        {
            States.Add(newState);
            newState.UpdateModelComponentNameEventListener(this);
            RebuildDropdownOptions();
        }

        public void RemoveStateCallback(object sender, State state)
        {
            States.Remove(state);
            state.UpdateModelComponentNameEventListener(this, false);
        }

        public void UpdateModelComponentNameCallback(object sender, string newName)
        {
            State state = sender as State;

            int stateValue = ModelComponentUniqueIDtoDropDownValue[state.UniqueID];
            GetComponent<TMP_Dropdown>().options[stateValue].text = newName;

            if (stateValue == Value)
            {
                GetComponent<TMP_Dropdown>().captionText.text = newName;
            }
        }
        #endregion


        void AddModelStatesToDropdown()
        {
            foreach (var state in Model.StateDictionary.Values)
            {
                States.Add(state as State);
                state.UpdateModelComponentNameEventListener(this);
            }
        }

        public void RebuildDropdownOptions()
        {
            int DropDownValue = 1;
            DropdownOptions.Clear();
            DropDownValueToModelComponent.Clear();
            ModelComponentUniqueIDtoDropDownValue.Clear();
            DropdownOptions.Add("");
            foreach (State state in States)
            {
                DropdownOptions.Add(state.Name);
                ModelComponentUniqueIDtoDropDownValue.Add(state.UniqueID, DropDownValue);
                DropDownValueToModelComponent.Add(DropDownValue++, state);
            }

            GetComponent<TMP_Dropdown>().ClearOptions();
            GetComponent<TMP_Dropdown>().AddOptions(DropdownOptions);
            SetValueWithoutNotify(Value);

        }
    }
}
