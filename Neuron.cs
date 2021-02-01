using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public List<double> Weights = new List<double>();
    double Bias = 0;

    int actCount = 0;

    public Neuron(int NumOfNeurons, bool IsInput)
    {
        Debug.Log("Creating Neuron with: " +NumOfNeurons.ToString());

        double rand = 0;

        if (IsInput == false)
        {
            rand = UnityEngine.Random.Range(0.0f, 0.50f);
            Bias = rand;
        }

        for (int i = 0; i < NumOfNeurons; i++)
        {
            rand = UnityEngine.Random.Range(0.00f, 1.2f);
            Weights.Add(rand);
        }
    }

    public double Activate(double[] inputs)
    {
        double SumInputs = 0;

        /*foreach(double d in inputs)
        {
            Debug.Log(d);
        }*/

        for (int i = 0; i < inputs.Length; i++)
        {
            //Debug.Log("before:" + inputs[i].ToString());
            //Debug.Log(Weights[i]);
            inputs[i] *= Weights[i];
            //Debug.Log("after:" + inputs[i].ToString());
        }

        foreach (double i in inputs)
        {
            SumInputs += i;
        }

        SumInputs += Bias;

        //actCount++;
        //Debug.Log(actCount);
        return Math.Log(1 + Math.Pow(Math.E, (SumInputs)));
    }
}
