using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace JorisHoef.Controllers
{
    [DisallowMultipleComponent]
    public class MouseLook : MonoBehaviour
    {
#region Constants and Fields
        private float _yaw, _pitch;
#endregion

#region Serialized Fields
        [Header("Targets")]
        [SerializeField]
        [Tooltip("Yaw/pan rotates this (usually a parent pivot). If null, auto-creates a YawPivot.")]
        private Transform _yawTarget;

        [SerializeField]
        [Tooltip("Pitch/tilt rotates this (usually the camera). If null, uses this transform.")]
        private Transform _pitchTarget;

        [Header("Feel")]
        [SerializeField] [Range(0.01f, 5f)] private float _sensitivity = 0.12f;

        [SerializeField] [Range(0f, 89f)] private float _pitchClamp = 85f;
        [SerializeField] private bool _invertY;

        [Header("UX")]
        [SerializeField] private bool _lockCursorOnStart = true;

        [SerializeField] private Key _toggleLockKey = Key.Escape;

        [Header("Auto Setup")]
        [SerializeField]
        [Tooltip("If yawTarget == pitchTarget or yawTarget is null, make a separate yaw pivot automatically.")]
        private bool _autoCreateYawPivot = true;
#endregion

#region Unity Methods
        private void Awake()
        {
#if !ENABLE_INPUT_SYSTEM
        Debug.LogError("MouseLook requires the New Input System. Enable it in Player Settings and install com.unity.inputsystem.");
#endif
            ResolvePitchTarget();
            InitializeRig();
            InitializeCursor();
        }

        private void Update()
        {
            Vector2 delta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;

            _yaw += delta.x * _sensitivity;
            _pitch += (_invertY ? 1f : -1f) * delta.y * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, -_pitchClamp, _pitchClamp);

            if (Keyboard.current != null)
            {
                KeyControl key = Keyboard.current[_toggleLockKey];
                if (key != null && key.wasPressedThisFrame)
                {
                    SetCursorLocked(Cursor.lockState != CursorLockMode.Locked);
                }
            }
        }

        private void LateUpdate()
        {
            if (_yawTarget)
            {
                _yawTarget.localRotation = Quaternion.Euler(0f, _yaw, 0f);
            }
            if (_pitchTarget)
            {
                _pitchTarget.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus && _lockCursorOnStart)
            {
                SetCursorLocked(true);
            }
        }
#endregion

#region Private Methods
        private static float NormalizeAngle(float a)
        {
            a %= 360f;
            if (a > 180f)
            {
                a -= 360f;
            }
            return a;
        }

        private void ResolvePitchTarget()
        {
            if (_pitchTarget == null)
            {
                _pitchTarget = transform;
            }
        }

        private void InitializeRig()
        {
            if (_autoCreateYawPivot && (_yawTarget == null || _yawTarget == _pitchTarget))
            {
                CreateYawPivotFromPitch();
                return;
            }

            _pitch = NormalizeAngle(_pitchTarget.localEulerAngles.x);
            _yaw = NormalizeAngle((_yawTarget ? _yawTarget : _pitchTarget).localEulerAngles.y);
        }

        private void InitializeCursor()
        {
            if (_lockCursorOnStart)
            {
                SetCursorLocked(true);
            }
        }

        private void CreateYawPivotFromPitch()
        {
            Vector3 camPos = _pitchTarget.position;
            Quaternion camRot = _pitchTarget.rotation;

            Vector3 e = camRot.eulerAngles;
            _pitch = NormalizeAngle(e.x);
            _yaw = NormalizeAngle(e.y);

            Transform oldParent = _pitchTarget.parent;
            GameObject pivotGo = new GameObject($"{gameObject.name}_YawPivot");
            Transform pivot = pivotGo.transform;

            if (oldParent != null)
            {
                pivot.SetParent(oldParent, true);
            }

            pivot.SetPositionAndRotation(camPos, Quaternion.Euler(0f, _yaw, 0f));
            pivot.localScale = Vector3.one;

            _pitchTarget.SetParent(pivot, true);
            _pitchTarget.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

            _yawTarget = pivot;
        }

        private void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
#endregion
    }
}