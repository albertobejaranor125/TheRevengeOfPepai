using System.Collections;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    #region Variable
    private bool _isFixed;
    #endregion
    #region Unity Messages
    private void Awake()
    {
        _isFixed = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController2D player = collision.gameObject.GetComponent<PlayerController2D>();
        if (player != null && player.IsDashing && !_isFixed)
        {
            RepairMachine(player.LastDashDirection);
        }
    }
    #endregion
    #region Methods
    private void RepairMachine(Vector2 dir)
    {
        Debug.Log("Machine repared");
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, 0.4f);
        //transform.position += (Vector3)dir * 0.1f;
        StartCoroutine(Knockback(dir));
        _isFixed = true;
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
    }
    #endregion
}
