using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModelSpeedScript : MonoBehaviour
{
    Model model = new();
    int numStates = 100000;
    int iterations = 1;
    List<State> states = new();
    List<Rate> rates = new();
    List<Connection> connections = new();

    public void test()
    {


        for (int i = 0; i < numStates; i++)
        {
            states.Add(model.AddState(i.ToString(), 1000));
        }

        Rate r = model.AddRate("r", 0.1f);

        int j = 0;
        for (int i = 0; i < numStates; i++)
        {
            j = (i + 1) % numStates;
            connections.Add(model.AddConnection(states[i], states[j]));
            connections[i].RateEquation.AddRateEquationVariable(r);
        }

        long elapsedMs = 0;
        for (int i = 0; i < 1000; i++)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            runModel();
            elapsedMs += watch.ElapsedMilliseconds;
            Debug.Log(elapsedMs / (i + 1));
        }
        elapsedMs /= 100;
        Debug.Log("Final: " + elapsedMs);


    }

    void runModel()
    {
        //model.run_DTM(0.1f, iterations);
    }
}
