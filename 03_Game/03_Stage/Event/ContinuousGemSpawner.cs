using System.Collections;
using UnityEngine;

public class ContinuousGemSpawner : MonoBehaviour
{
    StagePlayer _player;
    Coroutine _spawnRoutine;

    const int _firstSpawnCount = 10;
    const float _spawnDelayTime = 2f;
    WaitForSeconds _spawnWait = new WaitForSeconds(_spawnDelayTime);


    public void StartSpawn(StagePlayer player)
    { 
        _player = player;

        FirstSpawn();
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    // 처음 주변에 깔려있는 스폰
    // 플레이어랑 가까이에 있으면 이벤트 구독, UI 스폰 타이밍 겹쳐서 문제 있음!
    public void FirstSpawn()
    {
        Camera cam = Camera.main;
        for (int i = 0; i < _firstSpawnCount; i++)
        {
            float minRadius = 5f;
            float maxRadius = 8f;

            Vector2 dir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minRadius, maxRadius);

            Vector3 spawnPos = _player.transform.position + new Vector3(dir.x * distance, dir.y * distance, 0f);

            GemManager.Instance.SpawnGem(GemPoolIndex.GreenGem, spawnPos, 1);
        }
    }

    IEnumerator SpawnRoutine()
    {
        Camera cam = Camera.main;

        // 레퍼런스 게임 기준, 영역 밖에 스폰이 되어서 이동해보면 스폰된 애가 있음
        while (true)
        {
            float camHeight = cam.orthographicSize;
            float camWidth = cam.aspect * camHeight;

            float outerPadding = 2f;

            Vector2 dir = Random.insideUnitCircle.normalized;

            float spawnX = (camWidth + outerPadding) * Mathf.Sign(dir.x);   // 양수, 음수인지만 체크해서
            float spawnY = Random.Range(-camHeight - outerPadding, camHeight + outerPadding);

            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            {
                spawnY = (camHeight + outerPadding) * Mathf.Sign(dir.y);
                spawnX = Random.Range(-camWidth - outerPadding, camWidth + outerPadding);
            }

            Vector3 spawnPos = _player.transform.position + new Vector3(spawnX, spawnY, 0f);

            GemManager.Instance.SpawnGem(GemPoolIndex.GreenGem, spawnPos, 1);

            yield return _spawnWait;
        }
    }





}
