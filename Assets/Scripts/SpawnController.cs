using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class SpawnController : MonoBehaviour
{
    [Header("Copter prefab")]
    public GameObject copter;

    public GameObject[] copters;

    private bool kolvo = false;
    private bool dist = false;

    public int kolvosensors = 0;

    private bool spawn = false;

    private int kolvo_s = 0;
    private int dist_s = 0;

    public float[] sensorserror = new float[9];

    private void Spawn(int kolvo, int dist, Vector3 posy)
    {
        double sqrt = Math.Sqrt(kolvo);

        if (sqrt%1==0)
        {
            int tekkolvo = 0;
            bool check = false;

            for (int x=(int)posy.x; x<sqrt*dist+posy.x; x+= dist)
            {
                for (int y = (int)posy.z; y < sqrt*dist+posy.z; y+= dist)
                {
                    if (tekkolvo == kolvo)
                    {
                        check = true;
                        break;
                    }
                    GameObject go = Instantiate(copter, new Vector3(x, 1, y), Quaternion.identity);
                    go.GetComponent<CopterID>().ID = tekkolvo;
                    tekkolvo++;
                }

                if (check)
                {
                    break;
                }
            }
        }
        else
        {
            int sqrtfix = (int)Math.Round(sqrt);
            int tekkolvo = 0;
            bool check = false;

            int oldposyx = (int)posy.x;
            int oldposyz = (int)posy.z;


            while (tekkolvo!=kolvo)
            {
                int f = 0;
                int ff = 0;
                for (int x = oldposyx; x < sqrtfix * dist+ oldposyx; x += dist)
                { 
                    for (int y=oldposyz;y<sqrtfix*dist+oldposyz;y+=dist)
                    {
                        if (tekkolvo == kolvo)
                        {
                            check = true;
                            ff = y;
                            break;
                        }
                        GameObject go = Instantiate(copter, new Vector3(x, 1, y), Quaternion.identity);
                        go.GetComponent<CopterID>().ID = tekkolvo;
                        tekkolvo++;
                        ff = y;
                    }

                    f = x;
                    
                }
                oldposyx = f+=dist;
                //oldposyz = ff + dist;
                
                if (check)
                {
                    break;
                }
            }
        }
    }

    public void Init(GameObject but)
    {
        TMP_InputField input = but.GetComponent<TMP_InputField>();
        if (but.name == "Kolvo_kopters")
        {
            if (input.text!="" && Convert.ToInt32(input.text) >= 1)
            {
                kolvo_s = Convert.ToInt32(input.text);
                kolvo = true;
                input.image.color = Color.green;
                input.enabled = false;
            }
            else
            {
                input.image.color = Color.red;
            }
           
        }
        else if (but.name == "Dist_kopters")
        {
            if (input.text != "" && Convert.ToInt32(input.text)>=2)
            {
                dist_s = Convert.ToInt32(input.text);
                dist = true;
                input.image.color = Color.green;
                input.enabled = false;
            }
            else
            {
                input.image.color = Color.red;
            }
        }

        if (kolvo && dist &&kolvosensors==9)
        {
            spawn = true;
        }
    }

    public void InitValueSensors(GameObject but)
    {
        TMP_InputField input = but.GetComponent<TMP_InputField>();
        int value;

        if (input.text != "" && Convert.ToInt32(input.text) >= 1)
        {
            value = Convert.ToInt32(input.text);
            sensorserror[but.GetComponent<SensorID>().ID] = value;
            input.image.color = Color.green;
            input.enabled = false;
            kolvosensors++;
        }
        else
        {
            input.image.color = Color.red;
        }

        if (kolvo && dist && kolvosensors == 9)
        {
            spawn = true;
        }

    }

    private void Update()
    {
        if (spawn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.DrawLine(ray.origin, hit.point);
                if (Input.GetMouseButtonDown(0))
                {
                    Spawn(kolvo_s, dist_s, hit.point);
                    copters = GameObject.FindGameObjectsWithTag("copter");
                    spawn = false;
                }
            }
        }
        
            
    }
}
