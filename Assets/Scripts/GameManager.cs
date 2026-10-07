using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [Header("Goal")]
    [SerializeField] private int targetMatureShrooms = 5;

    [Header("Timer")]
    [SerializeField] private float gameDuration = 60f;

    private float timeRemaining;
    private int matureShroomCount;

    private bool gameEnded = false;

    public int MatureShroomsCount => matureShroomCount;
    public int TargetMatureShrooms => targetMatureShrooms;
    public float TimeRemaining => timeRemaining;
    public bool GameEnded => gameEnded;

    public bool gameStarted = false;

    public bool GameStarted => gameStarted;

    public event Action<bool> OnGameEnded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeRemaining = gameDuration;

        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted || gameEnded)
        {
            return;
        }

        UpdateTimer();
        CountMatureShrooms();
    }


    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;

            CountMatureShrooms();

            if (matureShroomCount >= targetMatureShrooms)
            {
                WinGame();
            }
            else
            {
                LoseGame();
            }
        }
    }

    void CountMatureShrooms()
    {
        Shroom[] allShrooms = FindObjectsByType<Shroom>();

        int count = 0;

        foreach (Shroom shroom in allShrooms)
        {
            if (shroom.IsMature)
            {
                count++;
            }
        }

        matureShroomCount = count;
    }


    void WinGame()
    {
        if (gameEnded)
        {
            return;
        }
        gameEnded = true;

        // Freeze gameplay
        Time.timeScale = 0f;

        Debug.Log("YOU WIN!");

        OnGameEnded?.Invoke(true);
    }


    void LoseGame()
    {
        if (gameEnded)
        {
           return; 
        }
        gameEnded = true;

        // Freeze gameplay
        Time.timeScale = 0f;
        
        Debug.Log("TIME'S UP!");
        
        OnGameEnded?.Invoke(false);
    }


    public void StartGame()
    {
        gameStarted = true;

        Time.timeScale = 1f;

        Debug.Log("GAME STARTED!");
    }
}
