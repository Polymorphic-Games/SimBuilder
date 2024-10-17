using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRateEquationVariableHandler
{
    public void AddRateEquationVariableCallback(object sender, RateEquationVariable variable);

    public void RemoveRateEquationVariableCallback(object sender, RateEquationVariable variable);
}
