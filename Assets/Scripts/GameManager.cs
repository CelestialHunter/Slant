using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManagerData gmData;
    
    public PlayerControl player;
    public CameraFollow cam;

    private AudioSource audioSource;

    // GUI system => score, speed, death

    // camera system => death

    private List<GameObject> slopeQueue;

    [SerializeField]
    private Vector3 startingPosition = new Vector3(0, -5, 35);
    
    [SerializeField]
    private Vector3 nextSlopeOffset = new Vector3(0, -4, 10);

    private Vector3 nextSlopePosition = new Vector3(0, -5, 40);


    private bool scoreGiven = false;
    private int score = 0;


    // Start is called before the first frame update
    void Start()
    {
        initialGeneration(startingPosition);

        // start playing game music
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gmData.gameMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        Vector3 playerPos = player.gameObject.transform.position;
        generateNewSlope(playerPos);
        increaseScore(playerPos);
    }

    public void initialGeneration(Vector3 startPosition)
    {
        nextSlopePosition = startPosition;

        slopeQueue = new List<GameObject>();
        GameObject slope = Instantiate(gmData.slopes[Random.Range(0, gmData.slopes.Length)], nextSlopePosition, Quaternion.identity);
        // instantiate deathzone under slope as child of slope
        Instantiate(gmData.deathZone,
                    new Vector3(slope.transform.position.x, slope.transform.Find("endPoint").transform.position.y - 5, slope.transform.position.z),
                    Quaternion.identity,
                    slope.transform);
        slopeQueue.Add(slope);

        computeNextPosition();

        for (int i = 0; i < 9; i++)
        {
            addNewSlope();
        }
    }

    private void computeNextPosition()
    {
        GameObject slope = slopeQueue[slopeQueue.Count - 1];
        nextSlopePosition = slope.transform.Find("endPoint").transform.position + nextSlopeOffset;
    }
    
    private void generateNewSlope(Vector3 playerPos)
    {
        if (playerPos.z < slopeQueue[0].transform.Find("endPoint").transform.position.z) return;
        
        // destroy first slope
        Destroy(slopeQueue[0]);
        slopeQueue.RemoveAt(0);

        // generate new slope
        addNewSlope();

        scoreGiven = false;
    }

    private void addNewSlope()
    {
        GameObject slope;
        do
        {
            slope = gmData.slopes[Random.Range(0, gmData.slopes.Length)];
        } while (slopeQueue[slopeQueue.Count - 1].name.Contains(slope.name));
        slope = Instantiate(slope, nextSlopePosition, Quaternion.identity);
        // instantiate deathzone under slope as child of slope
        Instantiate(gmData.deathZone,
                    new Vector3(slope.transform.position.x, slope.transform.Find("endPoint").transform.position.y - 5, slope.transform.position.z),
                    Quaternion.identity,
                    slope.transform);
        slopeQueue.Add(slope);
        computeNextPosition();
    }

    private void increaseScore(Vector3 playerPos)
    {
        if (scoreGiven) return;
        if (playerPos.z > slopeQueue[0].transform.position.z)
        {
            // increase score
            score++;
            Debug.Log(score);
            scoreGiven = true;
        }
    }

    public void Death(int type)
    {
        audioSource.Stop();
        audioSource.loop = false;
        
        if (type == 0)
        {
            // collider death
            Debug.Log("Collider death");

            // set death sound
            audioSource.clip = gmData.collideDeathSound;            

            // start death animation
        }
        else if (type == 1)
        {
            // fall death
            Debug.Log("Fall death");

            // set death sound
            audioSource.clip = gmData.fallDeathSound;

            // stop camera follow
            cam.StopFollowing();
        }

        audioSource.Play();

        // wait 10 seconds, then destroy player
        StartCoroutine(DestroyPlayer());
    }

    IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(10);
        Destroy(player.gameObject);
    }    
}
