using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{
    public enum SpawnMode
    {
        Single,
        Continuous,
        Wave
    }

    [Header("Основные настройки")]
    public GameObject objectToSpawn; // Префаб объекта для спавна
    [SerializeField] public SpawnMode spawnMode = SpawnMode.Single;
    public Transform spawnParent; // Родитель для спавнящихся объектов (опционально)

    [Header("Позиция спавна")]
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    public bool spawnOnSurface = false;
    public LayerMask surfaceLayer;

    [Header("Настройки непрерывного спавна")]
    public float spawnInterval = 1f;
    public int maxObjects = 200;

    [Header("Настройки волнового спавна")]
    public int objectsPerWave = 25;
    public float waveInterval = 5f;
    public int totalWaves = 5;

    private int currentWave = 0;
    private int spawnedObjectsCount = 0;
    private float timer = 0f;

    void Start()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Не назначен объект для спавна!");
            enabled = false;
            return;
        }

        switch (spawnMode)
        {
            case SpawnMode.Single:
                SpawnObject();
                enabled = false; // Отключаем скрипт после единичного спавна
                break;
            case SpawnMode.Wave:
                StartCoroutine(WaveSpawner());
                break;
        }
    }

    void Update()
    {
        if (spawnMode != SpawnMode.Continuous) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            if (maxObjects <= 0 || spawnedObjectsCount < maxObjects)
            {
                SpawnObject();
                spawnedObjectsCount++;
                timer = 0f;
            }
        }
    }

    IEnumerator WaveSpawner()
    {
        while (currentWave < totalWaves || totalWaves <= 0)
        {
            currentWave++;
            Debug.Log($"Начало волны {currentWave}");

            for (int i = 0; i < objectsPerWave; i++)
            {
                SpawnObject();
                yield return new WaitForSeconds(0.5f); // Задержка между спавном объектов в волне
            }

            if (currentWave >= totalWaves && totalWaves > 0) break;

            yield return new WaitForSeconds(waveInterval); // Ожидание между волнами
        }
    }

    void SpawnObject()
    {
        Vector3 spawnPosition = CalculateSpawnPosition();
        Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

        if (spawnParent != null)
        {
            spawnedObject.transform.SetParent(spawnParent);
        }

        Debug.Log($"Объект создан в позиции: {spawnPosition}");
    }

    Vector3 CalculateSpawnPosition()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        ) + spawnAreaCenter;

        if (spawnOnSurface)
        {
            RaycastHit hit;
            if (Physics.Raycast(randomPoint + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, surfaceLayer))
            {
                randomPoint = hit.point;
            }
        }

        return randomPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawCube(spawnAreaCenter, spawnAreaSize);
    }
}