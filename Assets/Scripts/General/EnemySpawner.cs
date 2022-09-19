using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject archerVillagerPrefab, spearmanVillagerPrefab;
    [SerializeField] Transform enemies;
    Vector2 pos;
    int randNum;

    public float spawnTimer = 3f;

    private void Start()
    {
        StartCoroutine(SpawnVillagers());
    }

    private void Update()
    {
        if (Necromancer.instance.isFinished)
        {
            spawnTimer = 5f;
            return;
        }

        if (GameManager.Instance.isWaveEnded)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                StartCoroutine(SpawnVillagers());
                spawnTimer = 3;
            }
        }
    }

    IEnumerator SpawnVillagers()
    {
        if (GameManager.Instance.isWaveEnded)
        {
            pos = transform.position;
            randNum = Random.Range(1, 5);
            for (int i = 0; i < randNum; i++)
            {
                Instantiate(spearmanVillagerPrefab, pos, Quaternion.identity, enemies);
                yield return new WaitForSeconds(.3f);
            }
            randNum = Random.Range(1, 5);
            for (int i = 0; i < randNum; i++)
            {
                Instantiate(archerVillagerPrefab, pos, Quaternion.identity, enemies);
                yield return new WaitForSeconds(.3f);
            }
        }
    }
}
