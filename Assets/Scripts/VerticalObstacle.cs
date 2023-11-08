using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VerticalObstacle : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    [SerializeField]
    float highMargin = 8f;

    [SerializeField]
    public List<ObstacleTriplet> obstacleTriplets;

    public List<float[]> obstacleSpeeds;

    public List<float> obstacleHighMargins;

    public List<float> obstacleLowMargins;

    void Start()
    {
        obstacleSpeeds = new List<float[]>();
        obstacleLowMargins = Enumerable.Repeat(0, obstacleTriplets.Count).Select(x => (float)x).ToList();
        obstacleHighMargins = Enumerable.Repeat(0, obstacleTriplets.Count).Select(x => (float)x).ToList();


        for (int i = 0; i < obstacleTriplets.Count; i++)
        {
            obstacleSpeeds.Add(new float[obstacleTriplets[i].obstacles.Length]);
            for (int j = 0; j < obstacleTriplets[i].obstacles.Length; j++)
            {
                obstacleSpeeds[i][j] = speed;
            }
            obstacleLowMargins[i] = getLowestValueInTriple(obstacleTriplets[i].obstacles);
            obstacleHighMargins[i] = obstacleLowMargins[i] + highMargin;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < obstacleTriplets.Count; i++)
        {
            for (int j = 0; j < obstacleTriplets[i].obstacles.Length; j++)
            {
                obstacleTriplets[i].obstacles[j].Translate(0, obstacleSpeeds[i][j] * Time.deltaTime, 0, Space.Self);

                if (obstacleTriplets[i].obstacles[j].position.y < obstacleLowMargins[i])                   
                {
                    obstacleSpeeds[i][j] *= -1;
                    obstacleTriplets[i].obstacles[j].position = new Vector3(obstacleTriplets[i].obstacles[j].position.x, obstacleLowMargins[i], obstacleTriplets[i].obstacles[j].position.z);
                }

                if (obstacleTriplets[i].obstacles[j].position.y > obstacleHighMargins[i])
                {
                    obstacleSpeeds[i][j] *= -1;
                    obstacleTriplets[i].obstacles[j].position = new Vector3(obstacleTriplets[i].obstacles[j].position.x, obstacleHighMargins[i], obstacleTriplets[i].obstacles[j].position.z);
                }
            }
        }
    }

    float getLowestValueInTriple(Transform[] triple)
    {
        float lowestValue = triple[0].position.y;
        for (int i = 1; i < triple.Length; i++)
        {
            if (triple[i].position.y < lowestValue)
            {
                lowestValue = triple[i].position.y;
            }
        }
        return lowestValue;
    }

    [System.Serializable]
    public class ObstacleTriplet
    {
        public Transform[] obstacles;
    }
}
