using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    #region Variables
    public int Score { get; private set; }
    public static ScoreController Instance;
    [SerializeField] private TextMeshProUGUI _scoreText;
    #endregion
    #region Unity Messages
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Score = 0;
        _scoreText.text = "Score: " + Score;
    }
    #endregion
    #region Methods
    public void AddScore(int amount)
    {
        Score += amount;
        _scoreText.text = "Score: " + Score;
    }
    #endregion
}
