using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    #region Variables
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 6f;

    [Header("Sprint Settings")]
    [SerializeField] private float _sprintMultiplier = 1.75f;

    [Header("Jump Settings")]
    [SerializeField] private int _maxJumps = 2;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip _jumpSound;

    private Rigidbody2D _rb2D;
    private Vector2 _moveInput;
    private bool _isJumpPressed;
    private bool _isGrounded;
    private DashAbility _dashAbility;
    private bool _isSprinting;
    private int _currentJumps;
    private AudioSource _audioSource;
    public bool IsDashing { get; set; }
    public Vector2 LastDashDirection { get; set; }
    #endregion
    #region Unity Messages
    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _dashAbility = GetComponent<DashAbility>();
        _audioSource = GetComponent<AudioSource>();
        _isJumpPressed = false;
        _isGrounded = true;
        _isSprinting = false;
        _currentJumps = _maxJumps;
    }
    void Update()
    {
        if (!TimerScript.Instance.IsPlaying) return;
        if ( _isJumpPressed && _currentJumps > 0) //&& _isGrounded)
        {
            Jump();
            _currentJumps--;
            _isJumpPressed = false;
        }
    }
    private void FixedUpdate()
    {
        if (!TimerScript.Instance.IsPlaying) return;
        Move();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            int i = 0;
            bool foundGroundContact = false;
            while (i < collision.contactCount && !foundGroundContact)
            {
                ContactPoint2D contact = collision.GetContact(i);
                if(contact.normal.y > 0.5f)
                {
                    _isGrounded = true;
                    _currentJumps = _maxJumps;
                    foundGroundContact = true;
                }
                i++;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
    #endregion
    #region Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isJumpPressed = true;
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!TimerScript.Instance.IsPlaying) return;
        if (context.started)
        {
            Vector2 dir = Vector2.right;
            if(_moveInput.x > 0)
            {
                dir = Vector2.right;
            }
            else if(_moveInput.x < 0)
            {
                dir = Vector2.left;
            }
            _dashAbility.TryDash(dir);
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!TimerScript.Instance.IsPlaying) return;
        if (context.started)
        {
            _isSprinting = true;
        }
        else if (context.canceled)
        {
            _isSprinting = false;
        }
    }
    private void Move()
    {
        if (IsDashing) return;
        float currentSpeed = _moveSpeed;
        if (_isSprinting)
        {
            currentSpeed *= _sprintMultiplier;
        }
        _rb2D.linearVelocity = new Vector2(_moveInput.x * currentSpeed, _rb2D.linearVelocity.y);
    }
    private void Jump()
    {
        _audioSource.PlayOneShot(_jumpSound);
        _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, _jumpForce);
    }
    public Rigidbody2D GetRigidbody2D()
    {
        return _rb2D;
    }
    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }
    #endregion
}
