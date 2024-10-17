using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI : Model
{
    public SI()
    {
        State S = AddState("S", 10000);
        State I = AddState("I", 0);

        Rate g = AddRate("g", 0.001f);

        Connection SI = AddConnection(S, I);
        SI.RateEquation.AddRateEquationVariable(g);
        SI.RateEquation.AddRateEquationVariable(I);
    }

    public void LoadModel()
    {

    }


}
