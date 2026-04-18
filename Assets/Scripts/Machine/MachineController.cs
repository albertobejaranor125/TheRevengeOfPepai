using System.Collections;
using System.Threading;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    #region Variable
    [Header("Machine Settings")]
    [SerializeField] private float _shakeAmount = 0.08f;
    [SerializeField] private Sprite _reparedMachine;
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _repairSound;
    private bool _isFixed;
    private Vector3 _positionMachine;
    private SpriteRenderer _spriteRenderer;
    public static System.Action OnMachineDestroyed;
    private float _spawnTime;
    private AudioSource _audioSource;
    #endregion
    #region Unity Messages
    private void Awake()
    {
        _isFixed = false;
        _positionMachine = transform.localPosition;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spawnTime = Time.time;
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!_isFixed)
        {
            IdleBroken();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController2D player = collision.gameObject.GetComponent<PlayerController2D>();
        if (player != null && player.IsDashing && !_isFixed)
        {
            RepairMachine(player.LastDashDirection);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController2D player = collision.gameObject.GetComponent<PlayerController2D>();
        if (player != null && player.IsDashing && !_isFixed)
        {
            RepairMachine(player.LastDashDirection);
        }
    }
    #endregion
    #region Methods
    private void IdleBroken()
    {
        Vector3 noiseMachine = new Vector3(Mathf.PerlinNoise(Time.time*10f, 0f)-0.5f,
            Mathf.PerlinNoise(0f, Time.time * 10f) - 0.5f, 0f);
        transform.localPosition = _positionMachine + _shakeAmount * noiseMachine;
    }
    private void RepairMachine(Vector2 dir)
    {
        Debug.Log("Machine repared");
        int points = 1;
        float timeAlive = Time.time - _spawnTime;
        if(timeAlive <= 2f)
        {
            points = 2;
        }
        ScoreController.Instance.AddScore(points);
        StartCoroutine(RepairAnimation());
        StartCoroutine(Knockback(dir));
        StartCoroutine(DisableAfterTimeFixed());
        _isFixed = true;
    }
    private IEnumerator RepairAnimation()
    {
        Vector3 start = Vector3.one;
        Vector3 target = Vector3.one * 2f;
        float timer = 0.0f;
        float duration = 0.2f;
        while (timer < duration)
        {
            float t = timer / duration;
            float scale = Mathf.Lerp(1f, 2f, 1 - Mathf.Pow(1 - t, 2));
            transform.localScale = start * scale;
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = target;
        _spriteRenderer.sprite = _reparedMachine;
        _audioSource.PlayOneShot(_repairSound);
    }
    private IEnumerator Knockback(Vector2 dir)
    {
        Vector3 start = transform.position;
        Vector3 target = start + (Vector3)dir * 0.2f;
        float timer = 0.0f;
        float duration = 0.1f;
        while (timer < duration)
        {
            transform.position = Vector3.Lerp(start, target, timer/duration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        _spriteRenderer.sortingOrder = 2;
    }
    private IEnumerator DisableAfterTimeFixed()
    {
        yield return new WaitForSeconds(2.5f);
        OnMachineDestroyed?.Invoke();
        gameObject.SetActive(false);
    }
    #endregion
}
