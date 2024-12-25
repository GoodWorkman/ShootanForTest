using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _accelerationTime = 3f;
    [SerializeField] private float _decelerationTime = 3f;
    [SerializeField] private float _moveRotationAngle = 50f;

    private Rigidbody2D _rigidbody;
    private Coroutine _moveCoroutine;
   
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private WaitForSeconds _waitDecelerationTime;
    private float _paddingX;
    private float _paddingY;
    private float _time;
    private Vector2 _currentVelocity;
    private Quaternion _currentRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        _paddingX = size.x / 2;
        _paddingY = size.y / 2;
        _waitDecelerationTime = new WaitForSeconds(_decelerationTime);
    }

     public void Move(Vector2 moveInput)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine); 
        }
        
        _moveCoroutine = StartCoroutine(
            MoveCoroutine(_accelerationTime, moveInput.normalized * _moveSpeed,
                Quaternion.AngleAxis(_moveRotationAngle * moveInput.y, Vector3.right)));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }

    public void StopMove()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
       
        _moveCoroutine = StartCoroutine(MoveCoroutine(_decelerationTime, Vector2.zero, Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity, Quaternion moveRotation)
    {
        _time = 0f;
        _currentVelocity = _rigidbody.velocity;
        _currentRotation = transform.rotation;

        while (_time< 1)
        {
            _time += Time.fixedDeltaTime / time;
            _rigidbody.velocity = Vector2.Lerp(_currentVelocity, moveVelocity, _time);
            transform.rotation = Quaternion.Lerp(_currentRotation, moveRotation, _time);
            yield return _waitForFixedUpdate;
        }
    }

    private IEnumerator MovePositionLimitCoroutine()
    {
        while (gameObject.activeSelf)
        {
            transform.position = ViewPort.Instance.PlayerMoveablePosition(
                transform.position, _paddingX, _paddingY);
            yield return null;
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return _waitDecelerationTime;
        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }
}