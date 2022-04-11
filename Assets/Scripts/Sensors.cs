using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour
{
    [Header("Sensors")]
    public float[] gps = new float[2];

    public float barometer;
    public float ultrazvuk;
    public List<RModuls> moduls = new List<RModuls>();
    public Vector3 gyro;
    public Vector3 acselerometer;
    public string kompass;
    public float speed;

    private GameObject gameManager;
    private GameObject[] copters;

    private Vector3 oldposy;
    private Vector3 oldrotation;

    private Vector3 posy;
    private float radius;


    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        radius = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnController>().sensorserror[8];
        copters = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnController>().copters;
    }

    private void Update()
    {
        if (posy!=transform.position)
        {
            GetSpeed();
            GetUltrazvuk();
            GetCompass();
            GetBarometr();
            GetAcselerometer();
            GetGyroscop();
            GetGPS();
            posy = transform.position;
        }
        SvModul(radius);
    }

    public void GetCompass()
    {
        int error = (int)gameManager.GetComponent<SpawnController>().sensorserror[6];
        float random = Random.Range(-error, error);

        float rad = transform.eulerAngles.y *(1 - (random/100));

        if (rad==0 || rad == 360)
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
        else if(rad < 90f)
        {
            kompass = "NE" + Mathf.Round(rad).ToString();
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

            float distance = Vector3.Distance(transform.position, hit.point) * (1 - (random / 100));
            if (distance <= 10f)
            {
                ultrazvuk = distance;
            }
            else
            {
                ultrazvuk = -1f;
            }
        }
    }
    public void GetBarometr()
    {
        int error = (int)gameManager.GetComponent<SpawnController>().sensorserror[1];
        float random = Random.Range(-error, error);

        float distance = (transform.position.y - 1.16f) * (1 - (random / 100));
        if (distance >= 10f)
        {
            barometer = distance;
        }
        else
        {
            barometer = -1f;
        }
       
    }
    public void GetAcselerometer()
    {

    }
    public void GetGyroscop()
    {
    }
    public void GetGPS()
    {
        int error = (int)gameManager.GetComponent<SpawnController>().sensorserror[0];
        float random = Random.Range(-error, error);

        float lat = transform.position.z;
        float longi = transform.position.x;

        gps[0] = lat * (1 - (random / 100));
        gps[1] = longi * (1 - (random / 100));
    }
    public void SvModul(float rad)
    {
        int error = (int)gameManager.GetComponent<SpawnController>().sensorserror[3];
        float random = Random.Range(-error, error);

        for (int i=0;i<copters.Length;i++)
        {
            if (copters[i]!=this.gameObject)
            {
                float Dist = Vector3.Distance(transform.position, copters[i].transform.position);
                string id = copters[i].GetComponent<CopterID>().ID.ToString();

                RModuls found = moduls.FindLast(item => item.ID == id);

                if (Dist <= rad && found == null)
                {
                    moduls.Add(new RModuls(id, Dist * (1 - (random / 100))));
                }
                else if (Dist <= rad && found != null)
                {
                    found.Dist = Dist * (1 - (random / 100));
                }
                else if (Dist > rad && found != null)
                {
                    moduls.Remove(found);
                }
            }
        }
    }
}

[System.Serializable]
public class RModuls
{
    public string ID = "";
    public float Dist = 0f;

    public RModuls(string IDs, float dists)
    {
        ID = IDs;
        Dist = dists;
    }
}