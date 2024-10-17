//using OpenCover.Framework.Model;
using SceneLeftPanel;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static ICommand;

namespace SceneLeftPanel
{
    public class RateContainerScript : MediatorComponent, IUpdateModelComponentNameHandler, IUpdateModelComponentValueHandler
    {

        public Model Model;
        public Rate Rate;

        public void OnDestroy()
        {
            Rate.UpdateModelComponentNameEventListener(this, false);
            Rate.UpdateModelComponentValueEventListener(this, false);
        }

        public void InitializeComponent(MediatorComponent mediator, Model model, Rate rate)
        {
            base.InitializeComponent(mediator);
            Model = model;
            Rate = rate;
            GetComponentInChildren<RateInputFieldNameScript>().InitializeComponent(this, rate);
            GetComponentInChildren<RateInputFieldValueScript>().InitializeComponent(this, rate);
            GetComponentInChildren<RemoveButtonScript>().InitializeComponent(this);

            Rate.UpdateModelComponentNameEventListener(this);
            Rate.UpdateModelComponentValueEventListener(this);
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case RemoveButtonScript:
                    commandStack.Push(new CommandRemoveRate(Model, Rate));
                    commandStack.Peek().Execute();
                    break;
                case RateInputFieldNameScript:
                    OnNotificationRateInputFieldNameScript(sender as RateInputFieldNameScript, args[0] as string);
                    //string rateName = (string)args[0];
                    //Rate.UpdateName(rateName);
                    break;
                case RateInputFieldValueScript:
                    string inputFieldText = ((RateInputFieldValueScript)sender).Text;
                    string newInputFieldText = (string)args[0];
                    float value;
                    if (float.TryParse(newInputFieldText, out value))
                    {
                        commandStack.Push(new CommandUpdateModelComponentValue(Rate, value));
                        commandStack.Peek().Execute();
                    }
                    else
                    {
                        GetComponentInChildren<RateInputFieldValueScript>().SetTextWithoutNotify(Rate.Value.ToString());
                    }
                    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
        }

        void OnNotificationRateInputFieldNameScript(RateInputFieldNameScript sender, string newName)
        {
            if (newName.Length > 1 || !Regex.IsMatch(newName, @"^[a-zA-z]+$"))
            {
                sender.SetTextWithoutNotify(sender.Text);
                return;
            }
            newName = newName.ToLower();
            foreach (var state in Model.RateDictionary.Values)
            {
                if (newName == state.Name)
                {
                    sender.SetTextWithoutNotify(sender.Text);
                    return;
                }
            }

            commandStack.Push(new CommandUpdateModelComponentName(Rate, newName));
            commandStack.Peek().Execute();
        }

        public void UpdateModelComponentNameCallback(object sender, string name)
        {
            GetComponentInChildren<RateInputFieldNameScript>().SetTextWithoutNotify(name);
        }

        public void UpdateModelComponentValueCallback(object sender, float value)
        {
            GetComponentInChildren<RateInputFieldValueScript>().SetTextWithoutNotify(value.ToString());
        }
    }
}
