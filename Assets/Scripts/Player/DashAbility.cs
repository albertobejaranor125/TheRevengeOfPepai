using System.Collections;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float _dashTime = 0.2f;
    [SerializeField] private float _dashSpeed = 15f;
    [SerializeField] private AudioClip _dashClip;
    private PlayerController2D _playerController2D;
    private bool _isDashing;
    private void Awake()
    {
        _playerController2D = GetComponent<PlayerController2D>();
        _isDashing = false;
    }
    public void TryDash(Vector2 dir)
    {
        if (!_isDashing)
        {
            StartCoroutine(Dash(dir));
        }
    }
    private IEnumerator Dash(Vector2 dir)
    {
        _isDashing = true;
        _playerController2D.IsDashing = true;
        _playerController2D.LastDashDirection = dir;
        _playerController2D.GetRigidbody2D().linearVelocity = dir * _dashSpeed;
        _playerController2D.GetAudioSource().PlayOneShot(_dashClip);
        yield return new WaitForSeconds(_dashTime);
        _playerController2D.IsDashing = false;
        _isDashing = false;
    }
}
