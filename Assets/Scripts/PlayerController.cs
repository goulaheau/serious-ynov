using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private const float Speed = 6f;
    private const float HeightJump = 1f;
    private const float Gravity = 3f;
    private const float LookSensitivity = 1f;

    [SerializeField] private Camera _camera;

    private Vector3 _movement;
    private float _rotationY;

    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        Move();
        Rotate();
    }
    
    private void Move()
    {
        var move = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            move.x -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            move.x += 1f;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            move.z -= 1f;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
        {
            move.z += 1f;
        }

        if (move.magnitude > 1.0f)
        {
            move /= move.magnitude;
        }

        _movement.x = move.x;
        _movement.z = move.z;

        if (Input.GetButtonDown("Jump") && _characterController.isGrounded)
        {
            _movement.y = HeightJump;
        }
        _movement.y -= Gravity * Time.deltaTime;

        _characterController.Move(transform.TransformDirection(_movement) * Speed * Time.deltaTime);
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * LookSensitivity);

        _rotationY -= Input.GetAxis("Mouse Y");
        _rotationY = Mathf.Clamp(_rotationY, -90, 90);

        _camera.transform.localEulerAngles = new Vector3(_rotationY, 0, 0);
    }
}