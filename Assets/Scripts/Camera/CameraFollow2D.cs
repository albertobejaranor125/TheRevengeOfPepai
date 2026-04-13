using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    private Vector3 _velocity;
    #endregion
    #region Unity Messages
    private void Awake()
    {
        if(_target == null)
        {
            _target = FindAnyObjectByType<PlayerController2D>().transform;
        }
    }
    private void LateUpdate()
    {
        Vector3 targetPosition = _target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, 0.1f);
    }
    #endregion
}
