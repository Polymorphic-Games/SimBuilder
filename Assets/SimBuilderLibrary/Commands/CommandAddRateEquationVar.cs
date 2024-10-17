using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAddRateEquationVar : ICommand
{
    RateEquation RateEquation;
    RateEquationVariable Variable;

    public CommandAddRateEquationVar(RateEquation rateEquation)
    {
        RateEquation = rateEquation;
    }

    public bool Execute()
    {
        Variable = RateEquation.AddRateEquationVariable();
        return true;
    }

    public void Undo()
    {
        RateEquation.RemoveRateEquationVariable(Variable);
    }

}
