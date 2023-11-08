using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerData", menuName = "ScriptableObjects/GameManagerData", order = 1)]
public class GameManagerData : ScriptableObject
{
    public GameObject[] slopes;
    public GameObject deathZone;

    public AudioClip gameMusic;
    public AudioClip collideDeathSound;
    public AudioClip fallDeathSound;
}
