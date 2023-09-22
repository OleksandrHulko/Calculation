using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private int _minAngle = 10;
    [SerializeField]
    private int _maxAngle = 80;
    [SerializeField]
    private int _minDistance = 30;
    [SerializeField]
    private int _maxDistance = 100;
    [Range(1, 50)] 
    [SerializeField] 
    private float _scrollSensitivity = 1;
    [Range(1, 10)] 
    [SerializeField] 
    private float _mouseSensitivity = 1;
    #endregion

    #region Private Fields
    private float _mouseXAxis = 0.0f;
    private float _mouseYAxis = 0.0f;
    private float _scrollAxis = 0.0f;

    private float _currentDistance = 0.0f;

    private bool _mouse0Pressed = false;
    #endregion
    
    
    #region Unity Methods
    private void Start()
    {
        _currentDistance = _maxDistance;
    }

    private void Update()
    {
        ReadInput();
        TryRotateCamera();
        Zooming();
    }
    #endregion

    #region Private Methods
    private void ReadInput()
    {
        _mouseXAxis    = Input.GetAxis("Mouse X")           * _mouseSensitivity;
        _mouseYAxis    = Input.GetAxis("Mouse Y")           * _mouseSensitivity;
        _scrollAxis    = Input.GetAxis("Mouse ScrollWheel") * _scrollSensitivity;
        _mouse0Pressed = Input.GetKey(KeyCode.Mouse0);
    }

    private void TryRotateCamera()
    {
        if (!_mouse0Pressed)
            return;
        
        transform.CorrectEulerAngles(Mathf.Clamp(transform.rotation.eulerAngles.x - _mouseYAxis, _minAngle, _maxAngle), _mouseXAxis, xSetMode: true);
    }

    private void Zooming()
    {
        _currentDistance = Mathf.Clamp(_currentDistance - _scrollAxis, _minDistance, _maxDistance);
        transform.position = -transform.forward * _currentDistance;
    }
    #endregion
}
