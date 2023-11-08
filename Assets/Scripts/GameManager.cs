using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManagerData gmData;
    
    public PlayerControl player;
    public CameraFollow cam;

    private AudioSource audioSource;

    public GameGUIScript gui;

    private Environment env;

    private List<GameObject> slopeQueue;
    
    [SerializeField]
    private Vector3 nextSlopeOffset = new Vector3(0, -5, 10);

    [SerializeField]
    private float xLeftLimit = -3.5f;

    [SerializeField]
    private float xRightLimit = 3.5f;

    private Vector3 nextSlopePosition = new Vector3(0, -5, 40);


    private bool scoreGiven = false;
    private int score = 0;

    private bool gamePaused = false;
    private bool soundOn = true;


    // Start is called before the first frame update
    void Start()
    {
        env = GetComponent<Environment>();
        initialGeneration();

        // start playing game music
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gmData.gameMusic;
        audioSource.loop = true;
        audioSource.Play();

        gui.StartCoroutine("initGui");
        
        

        SpeedScript.speed = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        Vector3 playerPos = player.gameObject.transform.position;
        generateNewSlope(playerPos);
        increaseScore(playerPos);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                gui.Resume();
                gamePaused = false;
            }
            else
            {
                gui.Pause();
                gamePaused = true;
            }
        }
    }

    public void initialGeneration()
    {
        slopeQueue = new List<GameObject>();
        slopeQueue.Add(GameObject.Find("StartSlope"));
        computeNextPosition();
        
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
        nextSlopePosition.x = Random.Range(xLeftLimit, xRightLimit);
    }
    
    private void generateNewSlope(Vector3 playerPos)
    {
        if (playerPos.z < slopeQueue[0].transform.Find("endPoint").transform.position.z + 10) return;
        
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
        env.generateNewEnv(slope.transform.position.z, slope.transform.position.y);
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

            gui.setScore(score);

            env.pruneEnv(playerPos);
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

            // wait 10 seconds, then destroy player
            StartCoroutine(DestroyPlayer());
        }

        audioSource.Play();
        gui.StartCoroutine("GameOver");
    }

    IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(10);
        Destroy(player.gameObject);
    }
    
    public void SpeedUp()
    {
        // call upon GUI to show speed up message
        gui.StartCoroutine("SpeedUp");
    }

    public void ToggleSound(UnityEngine.UI.Button button)
    {
        if(soundOn)
        {
            soundOn = false;
            audioSource.volume = 0;
            button.image.sprite = gmData.soundOff;
        }
        else
        {
            soundOn = true;
            audioSource.volume = 1;
            button.image.sprite = gmData.soundOn;
        }
    }
}
