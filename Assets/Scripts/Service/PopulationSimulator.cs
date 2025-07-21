using System;
using TMPro;
using UnityEngine;

public class PopulationSimulator : MonoBehaviour
{
    /// <summary>
    /// How fast the simulation will run.
    /// </summary>
    public float simulationSpeed = 30f;

    /// <summary>
    /// How long the simulation will run in days.
    /// </summary>
    public float simulationDuration = 30;

    /// <summary>
    /// The total time elapsed since the simulation started.
    /// </summary>
    private float timeKeep = 0f;

    /// <summary>
    /// The current population at the time of the last update.
    /// </summary>
    private int currentPopulation = 0;

    /// <summary>
    /// the initial population at the start of the simulation.
    /// </summary>
    private int initialPopulation = 0;

    /// <summary>
    /// The population growth rate chance percentage.
    /// </summary>
    public float populationGrowthRate = 0;

    /// <summary>
    /// The max number of individuals that can be supported by the environment.
    /// </summary>
    public int carryingCapacity = 0;

    /// <summary>
    /// The type of simulation to run.
    /// </summary>
    public SimType simulationType;

    /// <summary>
    /// A flag to indicate if the simulation has been configured correctly.
    /// </summary>
    private bool configured = false;

    /// <summary>
    /// The total time elapsed since the simulation started.
    /// </summary>
    public float TimeElapsed => timeKeep;

    /// <summary>
    /// The current population at the time of the last update.
    /// </summary>
    public int CurrentPopulation => currentPopulation;

    /// <summary>
    /// The simulation types available for the population simulator.
    /// </summary>
    public enum SimType : byte
    {
        Linear = 0,
        Exponential = 1,
        Logistic = 2,
        Decay = 3
    }

    private void Update()
    {
        if (!configured)
        {
            try
            {
                CheckConfig();
            }
            catch (ArgumentException e)
            {
                Debug.LogWarning(e.Message);
                return;
            }
        } else
        {
            switch (simulationType)
            {
                case SimType.Linear:
                    UpdateLinearSimulation();
                    break;

                case SimType.Exponential:
                    UpdateExponentialSimulation();
                    break;

                case SimType.Logistic:
                    UpdateLogisticSimulation();
                    break;

                case SimType.Decay:
                    UpdateDecaySimulation();
                    break;
            }

            timeKeep += Time.deltaTime;
        }
    }

    /// <summary>
    /// Ensures that the simulation configuration is valid before running the simulation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    private void CheckConfig()
    {
        if(simulationDuration == 0)
            throw new ArgumentException("Simulation duration must be set to a non-zero value.");

        if (populationGrowthRate == 0)
            throw new ArgumentException("Population growth rate must be set to a non-zero value.");

        if(carryingCapacity == 0)
            throw new ArgumentException("Carrying capacity must be set to a non-zero value.");

        if (initialPopulation == 0)
            throw new ArgumentException("Initial population must be set to a non-zero value.");

        configured = true;
    }

    /// <summary>
    /// The linear simulation update method.
    /// </summary>
    public void UpdateLinearSimulation()
    {

    }

    /// <summary>
    /// The exponential simulation update method.
    /// </summary>
    public void UpdateExponentialSimulation()
    {

    }

    /// <summary>
    /// The logistic simulation update method.
    /// </summary>
    public void UpdateLogisticSimulation()
    {

    }

    /// <summary>
    /// The decay simulation update method.
    /// </summary>
    public void UpdateDecaySimulation()
    {

    }
}
