using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class SSAsimple
{
    public void tau_adaptive(ModelSpatial model, float tau = 1)
    {

        if (tau < 1e-100)
        {
            System.Console.WriteLine("Error: tau leap step less than 1e-100\n");
            System.Environment.Exit(1);
        }

        foreach (State state in model.states)
        {
            state.NewValue = state.Value;
        }

        foreach (Connection entry in model.ConnectionDictionary.Values)
        {
            performEvent(tau, entry);
        }

        foreach (State entry in model.StateDictionary.Values)
        {
            if (entry.NewValue < 0)
            {
                for (int j = 0; j < 2; j++)
                {
                    tau_adaptive(model, tau / 2);
                }
                return;
            }
        }

        foreach (State state in model.states)
        {
            state.UpdatePopulation(state.NewValue);
        }

    }

    public void performEvent(float tau, Connection connection)
    {
        if (connection.FromState == null || connection.ToState == null)
        {
            return;
        }

        float rate = connection.RateEquation.CalculateRateEquation();
        int P = calculateP(tau, 1.0f, rate);
        connection.FromState.NewValue -= P;
        connection.ToState.NewValue += P;
    }

    public int calculateP(float tau, float population, float rate)
    {
        float mu = tau * System.Math.Abs(rate) * population;
        float P = 0;

        if (population < 0)
        {
            Debug.LogError("Negative population error");
        }
        if (mu == 0)
        {
            return 0;
        }
        else
        {
            P = PoissonCustom.GetPoisson(mu);
            //Debug.Log("P: " + P);
            if (rate >= 0)
            {
                return (int)P;
            }
            else
            {
                return (int)-P;
            }
        }
    }
}
