using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int playerKills;
    public int enemyKills;
    public TextMeshProUGUI playerKillCounter;
    public TextMeshProUGUI enemyKillCounter;
    public TextMeshProUGUI resultText;

    private void Awake()
    {

        if(Instance != null )
        {
            Destroy(gameObject);
            return;
        }

        Instance = this; 
        DontDestroyOnLoad(gameObject);

        if(PlayerPrefs.HasKey("playerKills"))
        {
            playerKills = PlayerPrefs.GetInt("0");
        }
        if (PlayerPrefs.HasKey("enemyKills"))
        {
            enemyKills = PlayerPrefs.GetInt("0");
        }
    }

    private void Update()
    {
        StartCoroutine(GameResult());
    }
    IEnumerator GameResult()
    {
        playerKillCounter.text = playerKills.ToString();
        enemyKillCounter.text = enemyKills.ToString();

        if (playerKills >= 10)
        {
            resultText.text = "Player Team Wins";
            PlayerPrefs.SetInt("playerKills", playerKills);
            Time.timeScale = 0;

            yield return new WaitForSeconds(5f);

            SceneManager.LoadScene("Arena");
        }
        else if (enemyKills >= 10)
        {
            resultText.text = "Enemy Team Wins";
            PlayerPrefs.SetInt("enemyKills", enemyKills);
            Time.timeScale = 0;

            yield return new WaitForSeconds(5f);

            SceneManager.LoadScene("Arena");
        }
    }
}
