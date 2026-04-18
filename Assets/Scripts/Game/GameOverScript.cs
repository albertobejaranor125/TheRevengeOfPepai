using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    #region Variables
    [Header("Game Over Settings")]
    [SerializeField] private GameObject _scoreController;
    [SerializeField] private GameObject _timerController;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _restartButton;
    [Header("Audio Settings")]
    [SerializeField] private AudioSource _audioAmbience;
    [SerializeField] private AudioClip _gameOverSound;
    private bool _gameOverShown;
    #endregion
    #region Unity Messages
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _scoreController.SetActive(true);
        _timerController.SetActive(true);
        _gameOverPanel.SetActive(false);
        _restartButton.SetActive(false);
        _gameOverShown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!TimerScript.Instance.IsPlaying && !_gameOverShown)
        {
            ShowGameOver();
        }
    }
    #endregion
    #region Methods
    private void ShowGameOver()
    {
        _audioAmbience.Stop();
        _audioAmbience.clip = _gameOverSound;
        _audioAmbience.loop = true;
        _gameOverShown = true;
        _scoreController.SetActive(false);
        _timerController.SetActive(false);
        _gameOverPanel.SetActive(true);
        _gameOverPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Game Over! You have saved many machines. Your score is " + ScoreController.Instance.Score + " points";
        _restartButton.SetActive(true);
        _audioAmbience.Play();
    }
    #endregion
}
