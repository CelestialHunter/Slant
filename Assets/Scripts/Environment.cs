using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{

    [SerializeField]
    private GameManagerData gmData;

    [SerializeField]
    private float leftBigX = -50;

    [SerializeField]
    private float rightBigX = 50;

    [SerializeField]
    private float leftSmallX = -30;

    [SerializeField]
    private float rightSmallX = 30;


    [SerializeField]
    private float startY = 0;

    [SerializeField]
    private float stepY = 10;

    [SerializeField]
    private float startZ = 10;

    [SerializeField]
    private float stepZ = 20;


    private float currentY;
    private float currentZ;

    private List<GameObject> envObjects;

    public void Start()
    {
        currentY = startY;
        currentZ = startZ;

        envObjects = new List<GameObject>();
        generateEnv();
    }



    public void generateEnv()
    {
        //for (int i = 0; i < 10; i++)
        //{
        //    generateNewEnv(currentY);
        //}
    }

    public void generateNewEnv(float z, float y)
    {
        if(envObjects == null)
            envObjects = new List<GameObject>();

        currentZ = z;
        for (int i = 0; i < 3; i++)
        {
            envObjects.Add(Instantiate(gmData.bigEnv, new Vector3(leftBigX, y - 10, currentZ + Random.Range(-.5f, .5f)), Quaternion.identity));
            setRandomAnimationFrame(envObjects[envObjects.Count - 1]);
            envObjects.Add(Instantiate(gmData.bigEnv, new Vector3(rightBigX, y - 10, currentZ + Random.Range(-.5f, .5f)), Quaternion.identity));
            setRandomAnimationFrame(envObjects[envObjects.Count - 1]);

            envObjects.Add(Instantiate(gmData.smallEnv, new Vector3(leftSmallX, y - 15, currentZ + Random.Range(-.5f, .5f)), Quaternion.identity));
            setRandomAnimationFrame(envObjects[envObjects.Count - 1]);
            envObjects.Add(Instantiate(gmData.smallEnv, new Vector3(rightSmallX, y - 15, currentZ + Random.Range(-.5f, .5f)), Quaternion.identity));
            setRandomAnimationFrame(envObjects[envObjects.Count - 1]);
                        
            currentZ += stepZ;
        }

        currentY -= stepY;
    }

    public void pruneEnv(Vector3 playerPos)
    {
        while (envObjects.Count > 0 && envObjects[0].transform.position.z < playerPos.z)
        {
            Destroy(envObjects[0]);
            envObjects.RemoveAt(0);
        }
    }

    private void setRandomAnimationFrame(GameObject obj)
    {
        Animator anim = obj.GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
