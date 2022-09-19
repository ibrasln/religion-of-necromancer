using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform enemies;

    public bool isWaveStarted;
    public bool isWaveEnded;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (enemies.childCount != 0)
        {
            isWaveStarted = true;
            isWaveEnded = false;
        }
        else
        {
            isWaveEnded = true;
            isWaveStarted = false;
        }
    }

}
