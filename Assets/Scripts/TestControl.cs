using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControl : MonoBehaviour
{
    public GameObject drone;

    private float h;
    private bool hh = false;

    private void Update()
    {
        if (drone!=null)
        {
            h = drone.GetComponent<Sensors>().barometer;
            if (h < 10)
            {
                if (!hh)
                {
                    drone.GetComponent<Rigidbody>().AddForce(transform.up * 20f);
                }
                
            }
            else
            {
                hh = true;
            }
        }
       
    }
}
