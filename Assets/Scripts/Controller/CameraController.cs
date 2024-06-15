using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _baseCameraSpeed;
    [SerializeField] private float _cameraMouseSensivity;
    [SerializeField] private float _cameraBoostValue;
    [SerializeField] private float _maxCameraBoostValue;
    [SerializeField] private float _cameraSlowDownMultiplier = 0.5f;
    [SerializeField] private float _minimumCameraRunValue = 1f;
    [SerializeField] private float _maximumCameraRunValue = 1000f;

    private float _totalCameraRun = 1f;

    private void Update()
    {
        int rightMouseButtonNumber = 1;
        string xMouseAxis = "Mouse X";
        string yMouseAxis = "Mouse Y";

        if (Input.GetMouseButtonDown(rightMouseButtonNumber))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButton(rightMouseButtonNumber))
        {
            float mouseX = Input.GetAxis(xMouseAxis) * _cameraMouseSensivity;
            float mouseY = Input.GetAxis(yMouseAxis) * _cameraMouseSensivity;
            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }

        if (Input.GetMouseButtonUp(rightMouseButtonNumber))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

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
                baseInputPosition *= _baseCameraSpeed;
            }
        }

        baseInputPosition *= Time.deltaTime;
        transform.Translate(baseInputPosition);
    }

    private Vector3 GetBaseInput()
    {
        Vector3 inputDirection = new Vector3();
        KeyCode forwardInputKey = KeyCode.W;
        KeyCode backInputKey = KeyCode.S;
        KeyCode leftInputKey = KeyCode.A;
        KeyCode rightInputKey = KeyCode.D;

        if (Input.GetKey(forwardInputKey))
        {
            inputDirection += Vector3.forward;
        }

        if (Input.GetKey(backInputKey))
        {
            inputDirection += Vector3.back;
        }

        if (Input.GetKey(leftInputKey))
        {
            inputDirection += Vector3.left;
        }

        if (Input.GetKey(rightInputKey))
        {
            inputDirection += Vector3.right;
        }

        return inputDirection;
    }
}