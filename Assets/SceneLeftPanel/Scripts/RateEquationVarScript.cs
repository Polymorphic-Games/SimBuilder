//using SceneLeftPanel;
//using SimBuilderLibrary;

//public class RateEquationVarScript : MediatorComponent, IUpdateRateEquationVariableModelComponentHandler, IUpdateModelComponentNameHandler
//{
//    Model Model;
//    RateEquationVariable RateEquationVariable;
//    public ModelComponent ModelComponent { get; set; }

//    public void OnDestroy()

//    {
//        RateEquationVariable.ModelComponent.UpdateModelComponentNameEventListener(this, false);
//    }

//    public void InitializeComponent(MediatorComponent mediator, Model model, RateEquationVariable rateEquationVariable)
//    {
//        base.InitializeComponent(mediator);
//        Model = model;
//        RateEquationVariable = rateEquationVariable;

//        GetComponentInChildren<RateEquationVarInputFieldScript>().InitializeComponent(this);

//        //update modelcomponentname event listener
//        RateEquationVariable.UpdateRateEquationVariableModelComponentEventListener(this);
//    }

//    public override void Notify(object sender, params object[] args)
//    {
//        switch (sender)
//        {
//            case RateEquationVarInputFieldScript:
//                OnNotificationRateEquationVarInputFieldScript((RateEquationVarInputFieldScript)sender, args);
//                break;
//            default:
//                base.Notify(sender, args);
//                break;
//        }
//    }

//    void OnNotificationRateEquationVarInputFieldScript(RateEquationVarInputFieldScript sender, params object[] args)
//    {
//        string ModelComponentName = (string)args[0];
//        ModelComponent NewModelComponent = null;

//        NewModelComponent = (State)Model.GetDictionaryValue(ModelComponentName, Model.StateDictionary);
//        if (NewModelComponent == null)
//        {
//            NewModelComponent = (Rate)Model.GetDictionaryValue(ModelComponentName, Model.RateDictionary);
//        }
//        if (NewModelComponent == null)
//        {
//            GetComponentInChildren<RateEquationVarInputFieldScript>().setText(sender.Text);
//            return;
//        }


//        commandStack.Push(new CommandUpdateRateEquationVar(RateEquationVariable, NewModelComponent));
//        commandStack.Peek().Execute();
//    }

//    public void UpdateRateEquationVariableModelComponentCallback(object sender, ModelComponent newModelComponent)
//    {
//        RateEquationVariable rateEquationVariable = (RateEquationVariable) sender;
//        rateEquationVariable.ModelComponent?.UpdateModelComponentNameEventListener(this, false);
//        newModelComponent?.UpdateModelComponentNameEventListener(this);
//        GetComponentInChildren<RateEquationVarInputFieldScript>().setText(newModelComponent?.Name);
//    }

//    public void UpdateModelComponentNameCallback(object sender, string componentName)
//    {
//        GetComponentInChildren<RateEquationVarInputFieldScript>().setText(componentName);
//    }

//}
