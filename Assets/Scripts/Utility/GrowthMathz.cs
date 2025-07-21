using System;
using UnityEngine;

/// <summary>
/// The math utility class for population growth calculations.
/// </summary>
public static class GrowthMathz 
{
    /// <summary>
    /// Calculates the population at a given time using the linear growth model.
    /// </summary>
    /// <returns>The population at a specific time</returns>
    private static int CalculateLinearPopulationForTime(int currentPopulation, float growthRate, float time)
    {
        int population = Mathf.RoundToInt(currentPopulation + growthRate * time);
        return population;
    }

    /// <summary>
    /// Calculates the population at a given time using the exponential growth model.
    /// </summary>
    /// <returns>The population at a specific time</returns>
    private static int CalculateExponentialPopulationForTime(int currentPopulation, float growthRate, float time)
    {
        int population = Mathf.RoundToInt(currentPopulation * Mathf.Exp(growthRate * time));

        return population;
    }

    /// <summary>
    /// Calculates the population at a given time using the logistic growth model.
    /// </summary>
    /// <returns>The population at a specific time</returns>
    private static int CalculateLogisticPopulationForTime(int currentPopulation, float growthRate, float time, float carryingCapacity)
    {
        float exponent = -growthRate * time;
        float denominator = 1f + ((carryingCapacity - currentPopulation) / currentPopulation) * Mathf.Exp(exponent);

        int population = Mathf.RoundToInt(carryingCapacity / denominator);
        return population;
    }

    /// <summary>
    /// Creates a population map for linear growth over time.
    /// </summary>
    /// <param name="initialPopulation"></param>
    /// <param name="growthRate"></param>
    /// <param name="time"></param>
    /// <param name="timeIterations"></param>
    /// <param name="totalTime"></param>
    /// <returns></returns>
    public static int[] CalculateLinearPopulationMap(float initialPopulation, float growthRate, int timeIterations, float totalTime)
    {
        int[] populationMap = new int[timeIterations];

        int currentPopulation = Mathf.RoundToInt(initialPopulation);

        for (int i = 0; i < timeIterations; i++)
        {
            float currentTime = totalTime / timeIterations * i;
            populationMap[i] = CalculateLinearPopulationForTime(currentPopulation, growthRate, currentTime);
        }

        return populationMap;
    }

    /// <summary>
    /// Creates a population map for exponential growth over time.
    /// </summary>
    /// <param name="initialPopulation"></param>
    /// <param name="growthRate"></param>
    /// <param name="time"></param>
    /// <param name="timeIterations"></param>
    /// <param name="totalTime"></param>
    /// <returns></returns>
    public static int[] CalculateExponentialPopulationMap(float initialPopulation, float growthRate, int timeIterations, float totalTime)
    {
        int[] populationMap = new int[timeIterations];

        int currentPopulation = Mathf.RoundToInt(initialPopulation);

        for (int i = 0; i < timeIterations; i++)
        {
            float currentTime = totalTime / timeIterations * i;
            populationMap[i] = CalculateExponentialPopulationForTime(currentPopulation, growthRate, currentTime);
        }

        return populationMap;
    }

    /// <summary>
    /// Creates a population map for logistic growth over time.
    /// </summary>
    /// <param name="initialPopulation"></param>
    /// <param name="growthRate"></param>
    /// <param name="time"></param>
    /// <param name="carryingCapacity"></param>
    /// <param name="timeIterations"></param>
    /// <param name="totalTime"></param>
    /// <returns></returns>
    public static int[] CalculateLogisticPopulationMap(float initialPopulation, float growthRate, float carryingCapacity, int timeIterations, float totalTime)
    {
        int[] populationMap = new int[timeIterations];

        int currentPopulation = Mathf.RoundToInt(initialPopulation);

        for (int i = 0; i < timeIterations; i++)
        {
            float currentTime = totalTime / timeIterations * i;
            populationMap[i] = CalculateLogisticPopulationForTime(currentPopulation, growthRate, currentTime, carryingCapacity);
        }

        return populationMap;
    }

}
