using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    #region Variables
    public int Score { get; private set; }
    public static ScoreController Instance;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _comboWindow = 2f;
    private float _lastRepairTime;
    private int _comboMultiplier;
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
        _lastRepairTime = 0f;
        _comboMultiplier = 1;
        Score = 0;
        _scoreText.text = "Score: " + Score;
    }
    #endregion
    #region Methods
    public void AddScore(int amount)
    {
        if(Time.time - _lastRepairTime > _comboWindow)
        {
            _comboMultiplier = 1;
        }
        else
        {
            _comboMultiplier++;
        }
        _lastRepairTime = Time.time;
        int finalAmount = amount * _comboMultiplier;
        Score += finalAmount;
        _scoreText.text = "Score: " + Score;
    }
    #endregion
}
