using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Windows;

public class SSAfast : MonoBehaviour
{
    private GridManagerScript gridManagerScript;

    public NativeArray<int> population;
    private NativeArray<int> rateEquationStateIndices;
    private NativeArray<int> populationNew;
    private NativeArray<int> fromIndices;
    private NativeArray<int> toIndices;
    private NativeArray<float> rates;
    private NativeArray<int> deltaP;

    private NativeArray<int> populationInfectedIndices;
    private NativeArray<int> populationInfected;
    private NativeArray<int> populationInfectedRow;
    private NativeArray<int> populationInfectedCol;

    public void run(GameObject canvasMap, int iterations, Model model)
    {
        gridManagerScript = canvasMap.GetComponent<GridManagerScript>();

        Data data = GetComponent<Data>();

        data.modelToNativeData(model);

        population = new NativeArray<int>(data.population.Length, Allocator.Persistent);
        for (int i = 0; i < data.population.Length; i++)
        {
            population[i] = data.population[i];
        }

        populationInfectedIndices = new NativeArray<int>(data.nameToPopStruct["I"].population.Count, Allocator.Persistent);
        populationInfected = new NativeArray<int>(data.nameToPopStruct["I"].population.Count, Allocator.Persistent);
        populationInfectedRow = new NativeArray<int>(data.nameToPopStruct["I"].population.Count, Allocator.Persistent);
        populationInfectedCol = new NativeArray<int>(data.nameToPopStruct["I"].population.Count, Allocator.Persistent);

        for (int i = 0; i < data.nameToPopStruct["I"].population.Count; i++)
        {
            populationInfectedIndices[i] = data.nameToPopStruct["I"].populationIndices[i];
            populationInfected[i] = data.nameToPopStruct["I"].population[i];
            populationInfectedRow[i] = data.nameToPopStruct["I"].row[i];
            populationInfectedCol[i] = data.nameToPopStruct["I"].col[i];
        }

        fromIndices = new NativeArray<int>(data.connections.Length, Allocator.Persistent);
        toIndices = new NativeArray<int>(data.connections.Length, Allocator.Persistent);
        rates = new NativeArray<float>(data.connections.Length, Allocator.Persistent);
        deltaP = new NativeArray<int>(data.connections.Length, Allocator.Persistent);

        for (int i = 0; i < data.connections.Length; i++)
        {
            fromIndices[i] = data.connections[i].From;
            toIndices[i] = data.connections[i].To;
            rates[i] = data.connections[i].Rate;
            deltaP[i] = 0;
        }

        rateEquationStateIndices = new NativeArray<int>(data.rateEquationStateVariableIndices.Length, Allocator.Persistent);
        for (int i = 0; i < data.rateEquationStateVariableIndices.Length; i++)
        {
            rateEquationStateIndices[i] = data.rateEquationStateVariableIndices[i];
        }

        populationNew = new NativeArray<int>(data.population.Length, Allocator.Persistent);

        //iterations = 100;
        var watch = new Stopwatch();
        watch.Start();
        for (int it = 0; it < iterations; it++)
        {
            tauLeapRecursive();
        }

        watch.Stop();
        UnityEngine.Debug.Log("Final: " + watch.ElapsedMilliseconds / (float)iterations);

        population.Dispose();
        populationNew.Dispose();
        populationInfectedIndices.Dispose();
        populationInfected.Dispose();
        populationInfectedRow.Dispose();
        populationInfectedCol.Dispose();
        rateEquationStateIndices.Dispose();
        fromIndices.Dispose();
        toIndices.Dispose();
        rates.Dispose();
        deltaP.Dispose();
    }

    private void tauLeapRecursive(float tauTop = 1, float tau = 1)
    {
        bool isNegative = false;

        var copyJob = new CopyJob
        {
            PopulationInput = population,
            PopulationOutput = populationNew
        };
        copyJob.Schedule(population.Length, 64).Complete();

        var deltaPJob = new DeltaPJob
        {
            Population = population,
            From = fromIndices,
            Rates = rates,
            RateEquationStateIndices = rateEquationStateIndices,
            DeltaP = deltaP,
            tau = tau
        };
        deltaPJob.Schedule(fromIndices.Length, 64).Complete();

        var updatePopulationJob = new UpdatePopulationJob
        {
            Population = populationNew,
            From = fromIndices,
            To = toIndices,
            DeltaP = deltaP
        };
        updatePopulationJob.Schedule().Complete();

        //var populationCheckJob = new PopulationCheckJob
        //{
        //    PopulationNew = populationNew,
        //    isNegative = false
        //};
        //populationCheckJob.Schedule().Complete();

        for (int i = 0; i < populationNew.Length; i++)
        {
            if (populationNew[i] < 0)
            {
                isNegative = true;
            }
        }
        if (isNegative)
        {
            for (int i = 0; i < 2; i++)
            {
                tauLeapRecursive(tauTop, tau / 2);
            }
            return;
        }
        else
        {
            var copyBackJob = new CopyJob
            {
                PopulationInput = populationNew,
                PopulationOutput = population
            };
            copyBackJob.Schedule(population.Length, 64).Complete();
        }

        //if (tau == tauTop)
        //{
        //    var updateInfectedPopulation = new UpdateInfectedPopulationJob
        //    {
        //        Population = population,
        //        PopulationInfectedIndices = populationInfectedIndices,
        //        PopulationInfected = populationInfected
        //    };
        //    updateInfectedPopulation.Schedule(populationInfectedIndices.Length, 64).Complete();

        //    for (int i = 0; i < populationInfected.Length; i++)
        //    {
        //        gridManagerScript.colorCell(
        //        populationInfectedRow[i],
        //        populationInfectedCol[i],
        //        populationInfected[i]);
        //    }
        //}
    }

    [BurstCompile]
    private struct PopulationCheckJob : IJob
    {
        [ReadOnly]
        public NativeArray<int> PopulationNew;

        //[WriteOnly]
        public bool isNegative;

        public void Execute()
        {
            isNegative = false;
            for (int i = 0; i < PopulationNew.Length; i++)
            {
                if (PopulationNew[i] < 0)
                {
                    isNegative = true;
                    return;
                }
            }
        }
    }

    [BurstCompile]
    private struct CopyJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<int> PopulationInput;

        [WriteOnly]
        public NativeArray<int> PopulationOutput;

        public void Execute(int i)
        {
            PopulationOutput[i] = PopulationInput[i];
        }
    }

    [BurstCompile]
    private struct DeltaPJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<int> Population;

        //[ReadOnly]
        //public NativeArray<Data.ConnectionComponent> Connections;
        [ReadOnly]
        public NativeArray<int> From;

        [ReadOnly]
        public NativeArray<float> Rates;

        [ReadOnly]
        public NativeArray<int> RateEquationStateIndices;

        [WriteOnly]
        public NativeArray<int> DeltaP;

        //public NativeArray<int> PopulationOut;
        public float tau;

        private int P;
        private float mu;
        private float propensity;

        public void Execute(int i)
        {
            propensity = (float)Population[From[i]] * tau;
            propensity *= Rates[i];
            for (int j = i * 5; j < i * 5 + 4; j++)
            {
                if (RateEquationStateIndices[j] == -1) { break; }
                propensity *= Population[RateEquationStateIndices[j]];
            }
            P = PoissonCustomBurst.GetPoisson(propensity);
            //P = 0; //timing test parameter
            DeltaP[i] = P;
        }
    }

    [BurstCompile]
    private struct UpdatePopulationJob : IJob
    {
        [ReadOnly]
        public NativeArray<int> From;

        [ReadOnly]
        public NativeArray<int> To;

        [ReadOnly]
        public NativeArray<int> DeltaP;

        public NativeArray<int> Population;

        public void Execute()
        {
            for (int i = 0; i < To.Length; i++)
            {
                Population[From[i]] -= DeltaP[i];
                Population[To[i]] += DeltaP[i];
            }
        }
    }

    [BurstCompile]
    private struct UpdateInfectedPopulationJob : IJobParallelFor
    {
        //private GridManagerScript gridManagerScript;

        [ReadOnly]
        public NativeArray<int> Population;

        [ReadOnly]
        public NativeArray<int> PopulationInfectedIndices;

        //[ReadOnly]
        //public NativeArray<int> populationInfectedRow;

        //[ReadOnly]
        //public NativeArray<int> populationInfectedCol;

        [WriteOnly]
        public NativeArray<int> PopulationInfected;

        public void Execute(int i)
        {
            //S00I00S01I01S10I10S11I11
            PopulationInfected[i] = Population[PopulationInfectedIndices[i]];
            //gridManagerScript.colorCell(
            //populationInfectedRow[i],
            //populationInfectedCol[i],
            //population[populationInfectedIndices[i]]);
        }
    }
}