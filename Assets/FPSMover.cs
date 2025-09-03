using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public class FPSMover : MonoBehaviour
{
#region Constants and Fields
    private CharacterController _cc;
    private Vector3 _velocity;
#endregion

#region Serialized Fields
    [Header("Orientation")]
    [SerializeField]
    [Tooltip("Forward/right basis for movement. If null, uses this transform.")]
    private Transform _orientation;

    [Header("Movement")]
    [SerializeField] [Range(0.5f, 20f)] private float _moveSpeed = 6f;
    [SerializeField] [Range(1f, 3f)] private float _sprintMultiplier = 1.6f;
    [SerializeField] private bool _enableSprint = true;

    [Header("Jump & Gravity")]
    [SerializeField] [Range(0.5f, 3f)] private float _jumpHeight = 1.3f;
    [SerializeField] [Range(5f, 40f)] private float _gravity = 19.62f;
    [SerializeField] private bool _enableJump = true;
    [SerializeField] [Range(0f, 5f)] private float _groundStick = 2f;
#endregion

#region Unity Methods
    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        if (_orientation == null)
            _orientation = transform;
    }

    private void Update()
    {
        //TODO: Check why camera in LateUpdate but not our movement
        Vector2 moveInput = ReadMoveInput();
        var sprint = _enableSprint && IsSprintHeld();// && _cc.isGrounded; TODO: Will probably stop velocity while in a jump.
        var jumpPressed = _enableJump && IsJumpPressed();

        //TODO: Check why it's called wish direction
        Vector3 wishDir = ComputeWishDirection(moveInput);
        var speed = sprint ? _moveSpeed * _sprintMultiplier : _moveSpeed;

        ApplyGroundAndGravity(jumpPressed);
        ApplyHorizontal(wishDir, speed);
    }
#endregion

#region Private Methods
    private Vector2 ReadMoveInput()
    {
        if (Keyboard.current == null)
            return Vector2.zero;

        float x = 0f, y = 0f;
        if (Keyboard.current.aKey.isPressed)
            x -= 1f;
        if (Keyboard.current.dKey.isPressed)
            x += 1f;
        if (Keyboard.current.sKey.isPressed)
            y -= 1f;
        if (Keyboard.current.wKey.isPressed)
            y += 1f;

        Vector2 v = new Vector2(x, y);
        return v.sqrMagnitude > 1f ? v.normalized : v;
    }

    private bool IsSprintHeld() => Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;

    private bool IsJumpPressed() => Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

    private Vector3 ComputeWishDirection(Vector2 moveInput)
    {
        Vector3 f = _orientation.forward;
        f.y = 0f;
        f.Normalize();
        Vector3 r = _orientation.right;
        r.y = 0f;
        r.Normalize();
        return f * moveInput.y + r * moveInput.x;
    }

    private void ApplyGroundAndGravity(bool jumpPressed)
    {
        if (_cc.isGrounded)
        {
            if (jumpPressed)
                _velocity.y = Mathf.Sqrt(2f * _gravity * _jumpHeight);
            else
                _velocity.y = -_groundStick;
        }
        else
            _velocity.y -= _gravity * Time.deltaTime;
    }

    private void ApplyHorizontal(Vector3 wishDir, float speed)
    {
        Vector3 horizontal = wishDir * speed;
        Vector3 motion = new Vector3(horizontal.x, _velocity.y, horizontal.z);
        _cc.Move(motion * Time.deltaTime);
    }
#endregion
}
