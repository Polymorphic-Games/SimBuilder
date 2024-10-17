using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSpatial : Model
{
    public List<State> states = new();
    public SSAsimple Driver = new();

    public void run_model(float tau, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            Driver.tau_adaptive(this, tau);
            //UpdateModelStatesEvent?.Invoke(this, StateDictionary);
        }
    }

}
