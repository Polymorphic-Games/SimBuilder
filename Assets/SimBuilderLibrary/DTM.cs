using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DTM : MonoBehaviour
{
    public void run(Model model, float tau = 1)
    {

        foreach (Connection connection in model.ConnectionDictionary.Values)
        {
            performEvent(tau, connection);
        }


        void performEvent(float tau, Connection connection)
        {
            if (connection.FromState == null || connection.ToState == null)
            {
                return;
            }

            float rate = connection.RateEquation.CalculateRateEquation();
            float P = rate * tau;

            if (connection.FromState.Value - P < 0)
            {
                connection.FromState.Value = 0;
                connection.ToState.Value += connection.FromState.Value;
            }
            else
            {
                connection.FromState.Value -= P;
                connection.ToState.Value += P;
            }

        }


    }
        
}
