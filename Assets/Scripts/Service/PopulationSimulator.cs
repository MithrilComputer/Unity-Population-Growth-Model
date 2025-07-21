using System;
using TMPro;
using UnityEngine;

public class PopulationSimulator : MonoBehaviour
{
    /// <summary>
    /// The maximum population that can be simulated.
    /// </summary>
    private int maxPopulation = 0;

    /// <summary>
    /// How fast the simulation will run.
    /// </summary>
    public float simulationSpeed = 1f;

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
    private SimType simulationType;

    /// <summary>
    /// The total time elapsed since the simulation started.
    /// </summary>
    public float TimeElapsed => timeKeep;

    /// <summary>
    /// The current population at the time of the last update.
    /// </summary>
    public int CurrentPopulation => currentPopulation;

    /// <summary>
    /// The current running state of the simulation.
    /// </summary>
    public bool IsRunning => isSimulationRunning;

    /// <summary>
    /// The current running state of the simulation.
    /// </summary>
    private bool isSimulationRunning = false;

    /// <summary>
    /// A flag to indicate if the simulation has been configured correctly.
    /// </summary>
    private bool configured = false;

    /// <summary>
    /// The simulation types available for the population simulator.
    /// </summary>
    private enum SimType : byte
    {
        Linear = 0,
        Exponential = 1,
        Logistic = 2,
    }

    /// <summary>
    /// The event that is triggered when the simulation ends.
    /// </summary>
    public event Action OnSimulationEnd;

    private void Update()
    {
        if(!isSimulationRunning)
        {
            return;
        }

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
        } 
        else
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
            }

            timeKeep += Time.deltaTime * simulationSpeed;
        }

        //Ensure the simulation does not run past the simulation duration
        float triggerTime = simulationDuration - Time.deltaTime * simulationSpeed;

        if (timeKeep >= triggerTime || currentPopulation <= 0 || currentPopulation >= maxPopulation)
        {
            timeKeep = simulationDuration;
            EndSimulation();
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
        currentPopulation = Mathf.RoundToInt(initialPopulation + populationGrowthRate * timeKeep);
    }

    /// <summary>
    /// The exponential simulation update method.
    /// </summary>
    public void UpdateExponentialSimulation()
    {
        int newPopulation = currentPopulation;

        for (int i = 0; i < currentPopulation; i++)
        {
            float chance = populationGrowthRate * Time.deltaTime * simulationSpeed;

            if (UnityEngine.Random.value < chance)
            {
                newPopulation++;
            }
        }

        currentPopulation = Mathf.Min(newPopulation, carryingCapacity);
    }

    /// <summary>
    /// The logistic simulation update method.
    /// </summary>
    public void UpdateLogisticSimulation()
    {
        int newPopulation = currentPopulation;

        // Calculate how close we are to capacity
        float capacityFactor = 1f - (currentPopulation / (float)carryingCapacity);

        // Base chance reduced by how close we are to full capacity
        float adjustedChance = populationGrowthRate * capacityFactor * Time.deltaTime * simulationSpeed;

        for (int i = 0; i < currentPopulation; i++)
        {
            if (UnityEngine.Random.value < adjustedChance)
            {
                newPopulation++;
            }
        }

        currentPopulation = Mathf.Min(newPopulation, carryingCapacity);
    }

    /// <summary>
    /// Sets the simulation to run in Linear mode and configures all the associated parameters.
    /// </summary>
    /// <param name="growthRate"></param>
    public void SetLinearSimulation(float populationGrowthRate, int initialPopulation, float simulationSpeed, float simulationDuration)
    {
        simulationType = SimType.Linear;

        this.simulationSpeed = simulationSpeed;
        this.simulationDuration = simulationDuration;

        this.populationGrowthRate = populationGrowthRate;
        this.initialPopulation = initialPopulation;

        configured = true;
    }

    /// <summary>
    /// Sets the simulation to run in Exponential mode and configures all the associated parameters.
    /// </summary>
    /// <param name="growthRate"></param>
    public void SetExponentialSimulation(float populationGrowthRate, int initialPopulation, int carryingCapacity, float simulationSpeed, float simulationDuration)
    {
        simulationType = SimType.Exponential;

        this.simulationSpeed = simulationSpeed;
        this.simulationDuration = simulationDuration;

        this.populationGrowthRate = populationGrowthRate;
        this.initialPopulation = initialPopulation;
        this.carryingCapacity = carryingCapacity;

        configured = true;
    }

    /// <summary>
    /// Sets the simulation to run in Logistic mode and configures all the associated parameters.
    /// </summary>
    /// <param name="growthRate"></param>
    public void SetLogisticSimulation(float populationGrowthRate, int initialPopulation, int carryingCapacity, float simulationSpeed, float simulationDuration)
    {
        simulationType = SimType.Logistic;

        this.simulationSpeed = simulationSpeed;
        this.simulationDuration = simulationDuration;

        this.populationGrowthRate = populationGrowthRate;
        this.initialPopulation = initialPopulation;
        this.carryingCapacity = carryingCapacity;

        configured = true;
    }

    /// <summary>
    /// Pauses the simulation, stopping any further updates until resumed.
    /// </summary>
    public void PauseSimulation()
    {
        if (!configured)
        {
            throw new ArgumentException("Simulation must be configured before pausing.");
        }

        if (!isSimulationRunning)
        {
            Debug.LogWarning("Simulation is already paused.");
            return;
        }
        isSimulationRunning = false;
    }

    /// <summary>
    /// Resumes the simulation from a paused state, allowing updates to continue.
    /// </summary>
    public void StartSimulation()
    {
        if (!configured)
        {
            throw new ArgumentException("Simulation must be configured before resuming.");
        }

        if (isSimulationRunning)
        {
            Debug.LogWarning("Simulation is already running.");
            return;
        }
        isSimulationRunning = true;
    }

    /// <summary>
    /// Sets the maximum population that can be simulated.
    /// </summary>
    /// <param name="population"></param>
    public void SetMaxPopulation(int population)
    {
        maxPopulation = population;
    }

    /// <summary>
    /// Ends the simulation, stopping all updates and finalizing the state.
    /// </summary>
    public void EndSimulation()
    {
        Destroy(gameObject);
        OnSimulationEnd?.Invoke();
    }
}
