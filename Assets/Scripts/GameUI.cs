using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goalText;
    [SerializeField] private TMP_Text timerText;

    [Header("End Game")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text score;

    void Start()
    {
        endGamePanel.SetActive(false);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameEnded += ShowEndGame;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        goalText.text = 
            "Mature Mushrooms: " + 
            GameManager.Instance.MatureShroomsCount;

        timerText.text = 
            "Time: " + 
            Mathf.CeilToInt(GameManager.Instance.TimeRemaining);
    }


    void ShowEndGame(bool playerWon)
    {
        endGamePanel.SetActive(true);

        if (playerWon)
        {
            title.text = "You win!";
        }
        else
        {
            title.text = "Time's up!";
        }

        score.text = "Mature mushrooms: " +
            GameManager.Instance.MatureShroomsCount +
            " / " +
            GameManager.Instance.TargetMatureShrooms;
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
