using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulKeeper : MonoBehaviour
{
    public int soul = 3;

    public static SoulKeeper instance;

    private void Awake()
    {
        instance = this;
    }

    public void ResetSoul()
    {
        soul = 0;
    }

    public void IncreaseSoul()
    {
        soul++;
        if (soul >= 3)
        {
            soul = 3;
        }
    }

    public int GetSoul()
    {
        return soul;
    }
}
