using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] List<Transform> spawns;

    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;

    void Start()
    {
        StartCoroutine(CoroutineStart());
    }

    IEnumerator CoroutineStart()
    {
        yield return new WaitForSeconds(5f);
        GameController.Instance.Canvas.StartPanel.SetActive(false);

        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnEnemy1(enemy1, Random.Range(0, 3)));
        StartCoroutine(SpawnEnemy1(enemy2, Random.Range(0, 3)));
        StartCoroutine(SpawnEnemy1(enemy3, Random.Range(0, 3)));
    }

    IEnumerator SpawnEnemy1(GameObject enemy, int indexSpawn)
    {
        while (GameController.Instance.Player.GetComponent<Hp>().CurrentHp > 0)
        {
            GameObject enemyClone = Instantiate(enemy, spawns[indexSpawn]);
            enemyClone.GetComponent<Hp>().Score = Mathf.RoundToInt(Random.Range(Time.timeSinceLevelLoad * 2, Time.timeSinceLevelLoad * 10));
            yield return new WaitForSeconds(Random.Range(3,6));
        }
    }
}
