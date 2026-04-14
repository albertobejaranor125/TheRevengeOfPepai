using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    #region Variables
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 6f;
    //public float MoveSmoothTime = 0.1f;

    [Header("Dash Settings")]
    private DashAbility _dashAbility;

    private Rigidbody2D _rb2D;
    private Vector2 _moveInput;
    private bool _isJumpPressed;
    private bool _isGrounded;
    public bool IsDashing { get; set; }
    public Vector2 LastDashDirection { get; set; }
    #endregion
    #region Unity Messages
    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _dashAbility = GetComponent<DashAbility>();
        _isJumpPressed = false;
        _isGrounded = true;
    }
    void Update()
    {
        if (!TimerScript.Instance.IsPlaying) return;
        if ( _isJumpPressed && _isGrounded)
        {
            Jump();
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
            _isGrounded = true;
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
            Vector2 dir = _moveInput;
            if(dir == Vector2.zero)
            {
                dir = Vector2.right;
            }
            _dashAbility.TryDash(dir);
        }
    }
    private void Move()
    {
        if (IsDashing) return;
        _rb2D.linearVelocity = new Vector2(_moveInput.x * _moveSpeed, _rb2D.linearVelocity.y);
    }
    private void Jump()
    {
        _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, _jumpForce);
    }
    public Rigidbody2D GetRigidbody2D()
    {
        return _rb2D;
    }
    #endregion
}
