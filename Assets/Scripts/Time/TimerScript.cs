using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private float TimeRemaining = 120f;
    [SerializeField] private TextMeshProUGUI TimerText;
    public static TimerScript Instance;
    public bool IsPlaying;
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
        IsPlaying = true;
    }
    void Update()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
        }
        else if (TimeRemaining <= 0)
        {
            TimeRemaining = 0;
            IsPlaying = false;
            Debug.Log("Tiempo acabado");
        }
        int minutes = Mathf.FloorToInt(TimeRemaining / 60);
        int seconds = Mathf.FloorToInt(TimeRemaining % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion
}
