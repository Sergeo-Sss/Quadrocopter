using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class SpawnController : MonoBehaviour
{
    [Header("Copter prefab")]
    public GameObject copter;

    private bool kolvo = false;
    private bool dist = false;

    private int kolvo_s = 0;
    private int dist_s = 0;

    private void Spawn(int kolvo, int dist)
    {
        double sqrt = Math.Sqrt(kolvo);
        if (sqrt%1==0)
        {
            for (int x=0; x<sqrt*dist; x+= dist)
            {
                for (int y = 0; y < sqrt*dist; y+= dist)
                {
                    Instantiate(copter, new Vector3(x, 0, y), Quaternion.identity);
                }
            }
        }
        else
        {
            int sqrtfix = (int)Math.Round(sqrt);
            int tekkolvo = 0;
            bool check = false;

            for (int x = 0; x < sqrtfix * dist; x += dist)
            {
                for (int y = 0; y < sqrtfix * dist; y += dist)
                {
                    Instantiate(copter, new Vector3(x, 0, y), Quaternion.identity);
                    tekkolvo++;
                    if (tekkolvo + 1 == kolvo)
                    {
                        Instantiate(copter, new Vector3(x, 0, y+=dist), Quaternion.identity);
                        check =true;
                        break;
                    }
                }
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
            if (input.text!="")
            {
                kolvo_s = Convert.ToInt32(input.text);
                kolvo = true;
                input.image.color = Color.green;
                input.enabled = false;
            }
           
        }
        else if (but.name == "Dist_kopters")
        {
            if (input.text != "")
            {
                dist_s = Convert.ToInt32(input.text);
                dist = true;
                input.image.color = Color.green;
                input.enabled = false;
            }
        }

        if (kolvo && dist)
        {
            Spawn(kolvo_s, dist_s);
        }
    }
}
