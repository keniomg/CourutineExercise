using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _baseCameraSpeed;
    [SerializeField] private float _cameraMouseSensivity;
    [SerializeField] private float _cameraBoostValue;
    [SerializeField] private float _maxCameraBoostValue;
    [SerializeField] private float _cameraSlowDownMultiplier = 0.5f;
    [SerializeField] private float _minimumCameraRunValue = 1f;
    [SerializeField] private float _maximumCameraRunValue = 1000f;

    private Vector3 _lastMousePosition = new Vector3(255, 255, 255);
    private float _totalCameraRun = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * _cameraMouseSensivity;
            float mouseY = Input.GetAxis("Mouse Y") * _cameraMouseSensivity;
            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        _lastMousePosition = Input.mousePosition;

        Vector3 baseInputPosition = GetBaseInput();

        if (baseInputPosition.sqrMagnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _totalCameraRun += Time.deltaTime;
                baseInputPosition = baseInputPosition * _totalCameraRun * _cameraBoostValue;
                baseInputPosition.x = Mathf.Clamp(baseInputPosition.x, -_maxCameraBoostValue, _maxCameraBoostValue);
                baseInputPosition.y = Mathf.Clamp(baseInputPosition.y, -_maxCameraBoostValue, _maxCameraBoostValue);
                baseInputPosition.z = Mathf.Clamp(baseInputPosition.z, -_maxCameraBoostValue, _maxCameraBoostValue);
            }
            else
            {
                _totalCameraRun = Mathf.Clamp(_totalCameraRun * _cameraSlowDownMultiplier, _minimumCameraRunValue, _maximumCameraRunValue);
                baseInputPosition = baseInputPosition * _baseCameraSpeed;
            }
        }

        baseInputPosition *= Time.deltaTime;
        transform.Translate(baseInputPosition);
    }

    private Vector3 GetBaseInput()
    {
        Vector3 inputDirection = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            inputDirection += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputDirection += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputDirection += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputDirection += new Vector3(1, 0, 0);
        }

        return inputDirection;
    }
}