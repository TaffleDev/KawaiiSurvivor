using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Player player;
    private WaveManagerUI ui;

    [Header("Settings")]
    [SerializeField] private float baseWaveDuration;
    [SerializeField] private float normalSpawnFrequencyMultiplier = 1f;
    [SerializeField] private float hyperSpawnFrequencyMultiplier = 2f;
    private float waveDuration;
    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    private List<float> localCounters = new List<float>();

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }

    void Start()
    {
    }

    void Update()
    {
        if (!isTimerOn)
            return;

        if (timer < waveDuration)
        {
            ManageCurrentWave();

            string timerString = ((int)(waveDuration - timer)).ToString();
            ui.UpdateTimerText(timerString);
        }
        else
            StartWaveTransition();
    }

    private void StartWave(int waveIndex)
    {
        ui.UpdateWaveText("Wave " + (currentWaveIndex + 1) + " / " + waves.Length);

        localCounters.Clear();

        foreach (WaveSegment segment in waves[waveIndex].segments)
            localCounters.Add(0);

        waveDuration = baseWaveDuration + Random.Range(1f, 5f); // Add 1-5 sec randomly

        timer = 0;
        isTimerOn = true;
    }

    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart || timer > tEnd)
                continue;

            float timeSinceSegmentStart = timer - tStart;
            
            float frequency = segment.spawnFrequency;
            float multiplier = waves[currentWaveIndex].isHyperWave ? hyperSpawnFrequencyMultiplier : normalSpawnFrequencyMultiplier;
            frequency *= multiplier; // Hyper wave spawns enemies twice as fast (you can tweak this)

            float spawnDelay = 1f / frequency;

            if (timeSinceSegmentStart / spawnDelay > localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;

                if (segment.spawnOnce)
                    localCounters[i] += Mathf.Infinity;
            }
        }

        timer += Time.deltaTime;
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;

        DefeatAllEnemies();

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            ui.UpdateTimerText(" ");
            ui.UpdateWaveText("Stage Completed!");

            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);
        }
        else
        {
            GameManager.instance.WaveCompletedCallback();
        }
    }

    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }

    private void DefeatAllEnemies()
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
            enemy.PassAwayAfterWave();
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized * Random.Range(6, 10);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -Constants.arenaSize.x / 2, Constants.arenaSize.x / 2);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -Constants.arenaSize.y / 2, Constants.arenaSize.y / 2);

        return targetPosition;
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;
            case GameState.GAMEOVER:
                isTimerOn = false;
                DefeatAllEnemies();
                break;
        }
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
    public bool isHyperWave; // NEW: Toggle for more intense waves
}

[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;
    public float spawnFrequency;
    public GameObject prefab;
    public bool spawnOnce;
}
