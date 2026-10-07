using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;

    public void StartGame()
    {
        startPanel.SetActive(false);

        GameManager.Instance.StartGame();
    }
}
