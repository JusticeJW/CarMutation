using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNet : MonoBehaviour
{
    public double Fitness;

    int p = 0;

    int NumOutsLocal = 0;
    int NumInNsLocal = 0;
    int NumHidLsLocal = 0;
    int NumHidsLocal = 0;

    public Neuron[] NeuronsLocal;
    Neuron[] Neurons;

    RaycastHit Ray;
    public LayerMask RayLayers;

    double[] RayDistances;
    bool[] RayHits;
    int[] RayAngles;

    public float MoveSpeed;
    public float rotSpeed;
    List<int> testThing = new List<int>();

    public bool Crashed;

    double temp;
    double Max;
    double Min;
    double Diff;
    // Start is called before the first frame update
    void Start()
    {
        RayDistances = new double[5];
        RayHits = new bool[5];
        RayAngles = new int[]{ 0, 1, 1, 1, 0, 1, 1, 0, -1, -1 };
        //MoveSpeed = 5.0f;
        Crashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Crashed == false)
        {
            p = 0;
            for (int i = 0; i < 5; i++)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(RayAngles[i + 5], 0, RayAngles[i])), out Ray, 1000.0f, RayLayers))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(RayAngles[i + 5], 0, RayAngles[i])) * Ray.distance, Color.yellow);
                    RayHits[i] = true;
                    RayDistances[i] = Ray.distance;
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(RayAngles[i + 5], 0, RayAngles[i])) * 1000, Color.white);
                    RayHits[i] = false;
                    RayDistances[i] = 0;
                }
            }

            foreach (bool i in RayHits)
            {
                if (i == true)
                    p++;
            }

            if (p == 5)
            {
                temp = 0;
                for (int i = 0; i < RayDistances.Length; i++)
                {
                    for (int j = 0; j < RayDistances.Length; j++)
                    {
                        if (j > 0)
                        {
                            if (RayDistances[j] < RayDistances[j - 1])
                            {
                                temp = RayDistances[j - 1];
                                RayDistances[j - 1] = RayDistances[j];
                                RayDistances[j] = temp;
                            }
                        }
                    }
                }

                Max = RayDistances[(RayDistances.Length - 1)];
                Min = RayDistances[0];
                Diff = Max - Min;

                for (int i = 0; i < RayDistances.Length; i++)
                {
                    RayDistances[i] = (RayDistances[i] - Min) / Diff;
                }

                // Debug.Log(((float)(FeedForward(RayDistances)[0]) - (float)(FeedForward(RayDistances)[1])));
                //Debug.Log(new Vector3(transform.rotation.x, ((float)(FeedForward(RayDistances)[0]) - (float)(FeedForward(RayDistances)[1])), transform.rotation.z) * rotSpeed);
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, ((float)(FeedForward(RayDistances)[0]) - (float)(FeedForward(RayDistances)[1])) * rotSpeed, transform.rotation.z));
            }

            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime, Space.Self);
            Fitness += Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.gameObject.tag == "Wall")
        {
            //Debug.Log("hitting wall");
            Crashed = true;
            transform.position = new Vector3(-40f, -1f, 75f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Environment.Instance.OnCrash();
        }
    }
    /*
    public NeuralNet(int NumIns, int NumInNs, int NumHidLs, int NumHids, int NumOuts)
    {
        NumOutsLocal = NumOuts;
        NumInNsLocal = NumInNs;
        NumHidLsLocal = NumHidLs;
        NumHidsLocal = NumHids;

        Neurons = new Neuron[(NumInNs + (NumHidLs * NumHids) + NumOuts)];

        for(int i = 0; i < NumInNs; i++)
        {
            Neurons[i] = new Neuron(NumIns);
        }

        for (int i = 0; i < NumHidLs; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < NumHids; j++)
                {
                    Neurons[j] = new Neuron(NumInNs);
                }
            }
            else
            {
                for (int j = 0; j < NumHids; j++)
                {
                    Neurons[j] = new Neuron(NumHids);
                }
            }
        }

        for (int i = 0; i < NumHids; i++)
        {
            Neurons[i] = new Neuron(NumHids);
        }

        NeuronsLocal = Neurons;
    }
    */

    public void SetProperties(int NumIns, int NumInNs, int NumHidLs, int NumHids, int NumOuts)
    {

        NumOutsLocal = NumOuts;
        NumInNsLocal = NumInNs;
        NumHidLsLocal = NumHidLs;
        NumHidsLocal = NumHids;

        Neurons = new Neuron[(NumInNs + (NumHidLs * NumHids) + NumOuts)];

        for (int i = 0; i < NumInNs; i++)
        {
            Neurons[i] = new Neuron(NumIns, true);
        }

        for (int i = NumIns; i < NumIns + NumHidLs; i++)
        {
            if (i == NumIns)
            {
                for (int j = NumIns; j < NumHids + NumIns; j++)
                {
                    Neurons[j] = new Neuron(NumInNs, false);
                }
            }
            else
            {
                for (int j = ((i - NumIns) * NumHids) + NumIns; j < (NumHids + ((i - NumIns) * NumHids) + NumIns); j++)
                {
                    Neurons[j] = new Neuron(NumHids, false);
                }
            }
        }

        for (int i = (NumIns +(NumHidLs * NumHids)); i < (NumIns + NumOuts + (NumHidLs * NumHids)); i++)
        {
            Neurons[i] = new Neuron(NumHids, false);
        }

        for(int i = 0;i < Neurons.Length; i++)
        {

        }

        NeuronsLocal = (Neuron[])Neurons.Clone();
    }


    public double[] FeedForward(double[] Ins)
    {
        double[] Outputs = new double[NumOutsLocal];
        double[] TempSynapses = new double[Ins.Length];
        double[] TempSynapses2 = new double[Ins.Length];

        int p = 0;


        for(int i = 0; i < NumInNsLocal; i++)
        {
            TempSynapses[i] = NeuronsLocal[p].Activate(Ins);
            p++;
        }

        for (int i = 0; i < NumHidLsLocal; i++)
        {
            if (i % 2 == 1)
            {
                TempSynapses2 = new double[NumHidsLocal];
            }

            if (i != 0 && i % 2 == 0)
            {
                TempSynapses = new double[NumHidsLocal];
            }

            if (i == 0)
            {
                for (int j = 0; j < NumHidsLocal; j++)
                {
                    TempSynapses2[j] = NeuronsLocal[p].Activate(TempSynapses);
                    p++;
                }
            }
            else if (i % 2 == 0)
            {
                for (int j = 0; j < NumHidsLocal; j++)
                {
                    TempSynapses[j] = NeuronsLocal[p].Activate(TempSynapses);
                    p++;
                }
            }
            else
            {
                for (int j = 0; j < NumHidsLocal; j++)
                {
                    TempSynapses2[j] = NeuronsLocal[p].Activate(TempSynapses);
                    p++;
                }
            }
        }

        if (NumHidLsLocal % 2 == 0)
        {
            for (int i = 0; i < NumOutsLocal; i++)
            {
                Outputs[i] = NeuronsLocal[p].Activate(TempSynapses2);
                p++;
            } 
        }
        else
        {
            for (int i = 0; i < NumOutsLocal; i++)
            {
                Outputs[i] = NeuronsLocal[p].Activate(TempSynapses);
                p++;
            }
        }

        return Outputs;
    }
}
