using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Environment : MonoBehaviour
{
    public static Environment Instance;
    public float simulationSpeed = 1.0f;

    int PopSize = 12;

    int NumIns = 5;
    int NumInNs = 5;
    int NumHidLs = 2;
    int NumHids = 5;
    int NumOuts = 2;

    public int MutationRate = 1;

    public Slider SimSlider;
    public GameObject Car;
    GameObject[] Cars;
    NeuralNet[] Population;

    int NumCrashes = 0;

    public void Awake()
    {
        if (Instance != null)
            Instance = this;
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cars = new GameObject[PopSize];
        Population = new NeuralNet[PopSize];
        for (int i = 0; i < PopSize; i++)
        {
            Cars[i] = Instantiate(Car, new Vector3(0, 0, 0), Quaternion.Euler(0,0,0));
            Cars[i].GetComponent<NeuralNet>().SetProperties(NumIns, NumInNs, NumHidLs, NumHids, NumOuts);
            Population[i] = Cars[i].GetComponent<NeuralNet>();

        }

        Time.timeScale = (simulationSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCrash()
    {
        NumCrashes++;
        if(NumCrashes == PopSize)
        {
            MutateChildPop(CrossPop(SortPop(Population)));
            foreach (GameObject i in Cars)
            {
                i.transform.position = new Vector3(0, 0, 0);
                i.GetComponent<NeuralNet>().Fitness = 0;
                i.GetComponent<NeuralNet>().Crashed = false;
            }
            NumCrashes = 0;
        }
    }

    public void OnSliderChanged()
    {
        simulationSpeed = SimSlider.value;
        Time.timeScale = (simulationSpeed);
    }

    public NeuralNet[] SortPop (NeuralNet[] Pop)
    {
        double temp = 0;

        for (int i = 0; i < PopSize; i++)
        {
            for (int j = 0; j < PopSize; j++)
            {
                if(j > 0)
                {
                    if (Pop[j].Fitness > Pop[j - 1].Fitness)
                    {
                        temp = Pop[j - 1].Fitness;
                        Pop[j - 1].Fitness = Pop[j].Fitness;
                        Pop[j].Fitness = temp;
                    }
                }
            }
        }
        return Pop;
    }

    public NeuralNet[] CrossPop (NeuralNet[] Pop)
    {
        int mn = 0;
        NeuralNet[] NewPop = new NeuralNet[Pop.Length];
        int rand = 0;

        for(int i = 0; i < Pop.Length/2; i++)
        {
            NewPop[i] = Pop[i];
        }

        for (int i = 0; i < Pop.Length / 2; i++)
        {
            NewPop[i + 6] = Pop[mn];
            for (int j = 0; j < Pop[mn].NeuronsLocal.Length; j++)
            {
                rand = Random.Range(0, 2);

                if (rand == 0)
                {
                    NewPop[i + 6].NeuronsLocal[j].Weights = Pop[mn + 1].NeuronsLocal[j].Weights;
                }
            }

            if(i % 2 == 1)
                mn++;
        }

        return NewPop;
    }

    public NeuralNet[] MutateChildPop (NeuralNet[] Pop)
    {
        double rand = 0;
        for (int i = 0; i < Pop.Length/2; i++)
        {
            for (int j = 0; j < Pop[i + 6].NeuronsLocal.Length; j++)
            {
                for (int k = 0; k < Pop[i + 6].NeuronsLocal[j].Weights.Count; k++)
                {
                    rand = Random.Range(0, MutationRate);
                    if (rand == 0)
                    {
                        rand = UnityEngine.Random.Range(-0.20f, 0.20f);
                        Pop[i + 6].NeuronsLocal[j].Weights[k] += rand;
                    }
                }
            }
        }

        return Pop;
    }
}
