using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour
{
    [Header("Sensors")]
    public float[] gps = new float[2];

    public float speed;
    public float ultrazvuk;
    public string kompass;

    private GameObject gameManager;

    private Vector3 oldposy;
    private Vector3 oldrotation;

    private Vector3 posy;
    private Vector3 rotation;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void Update()
    {
        posy = transform.position;


        GetSpeed();
        GetUltrazvuk();
        GetCompass();
    }

    public void GetCompass()
    {
        int error = (int)gameManager.GetComponent<SpawnController>().sensorserror[6];
        float random = Random.Range(-error, error);

        float rad = transform.eulerAngles.y *(1 - (random/100));

        if (rad==0)
        {
            kompass = "N";
        }
        else if (rad==90f)
        {
            kompass = "E";
        }
        else if (rad == 180)
        {
            kompass = "S";
        }
        else if (rad == 270f)
        {
            kompass = "W";
        }

        if (rad<90f)
        {
            kompass = "NE"+Mathf.Round(rad).ToString();
        }
        else if (rad > 90f && rad < 180f)
        {
            kompass = "SE" + Mathf.Round(rad).ToString();
        }
        else if (rad > 180f && rad < 270f)
        {
            kompass = "SW" + Mathf.Round(rad).ToString();
        }
        else if (rad > 270f && rad < 360f)
        {
            kompass = "NW" + Mathf.Round(rad).ToString();
        }
    }


    public void GetSpeed()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude < 0.001f)
        {
            speed = 0;
        }
        else
        {
            int error = (int)gameManager.GetComponent<SpawnController>().sensorserror[7];
            float random = Random.Range(-error, error);
            speed = GetComponent<Rigidbody>().velocity.magnitude * (1 - (random / 100));
        }
    }

    public void GetUltrazvuk()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            Debug.DrawLine(ray.origin, hit.point);
            int error = (int)gameManager.GetComponent<SpawnController>().sensorserror[2];
            float random = Random.Range(-error, error);
            ultrazvuk = Vector3.Distance(transform.position,hit.point)*(1-(random/100));

        }
    }
}
