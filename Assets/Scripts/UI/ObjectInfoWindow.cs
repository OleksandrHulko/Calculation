using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfoWindow : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField] 
    private GameObject _positionGroup = null;
    [SerializeField] 
    private GameObject _scaleGroup = null;
    [SerializeField] 
    private GameObject _rotationGroup = null;
    
    [SerializeField]
    private GameObject _deleteButton = null;
    
    [SerializeField]
    private InputFieldPro _positionXField = null;
    [SerializeField]
    private InputFieldPro _positionYField = null;
    [SerializeField]
    private InputFieldPro _positionZField = null;
    [SerializeField]
    private InputFieldPro _scaleXField = null;
    [SerializeField]
    private InputFieldPro _scaleYField = null;
    [SerializeField]
    private InputFieldPro _scaleZField = null;
    [SerializeField]
    private InputFieldPro _rotationYField = null;
    #endregion

    #region Private Fields
    private Transform _targetTransform = null;
    private int _layer = 0;
    #endregion


    #region Public Methods
    public void Init()
    {
        _positionXField.Init("X");
        _positionYField.Init("Y");
        _positionZField.Init("Z");
        
        _scaleXField.Init("X scale");
        _scaleYField.Init("Y scale");
        _scaleZField.Init("Z scale");
     
        _rotationYField.Init("Rotation");
    }
    
    public void Reinit( Transform targetTransform )
    {
        _targetTransform = targetTransform;

        _layer = targetTransform.gameObject.layer;
        
        bool useOnlyPosition = !_layer.IsBuildingLayer();
        bool isReceiver = _layer.IsReceiverLayer();

        gameObject.SetActive(true);

        _deleteButton.SetActive(!isReceiver);
        
        _scaleGroup   .SetActive(!useOnlyPosition);
        _rotationGroup.SetActive(!useOnlyPosition);

        _positionXField.Reinit(_targetTransform.position.x, delegate(string s) { _targetTransform.SetXPosition(s.ToInt()); });
        _positionYField.Reinit(_targetTransform.position.y, delegate(string s) { _targetTransform.SetYPosition(s.ToInt()); });
        _positionZField.Reinit(_targetTransform.position.z, delegate(string s) { _targetTransform.SetZPosition(s.ToInt()); });
        
        if(useOnlyPosition)
            return;

        _scaleXField.Reinit(_targetTransform.localScale.x, delegate(string s) { _targetTransform.SetXScale(s.ToInt()); });
        _scaleYField.Reinit(_targetTransform.localScale.y, delegate(string s) { _targetTransform.SetYScale(s.ToInt()); });
        _scaleZField.Reinit(_targetTransform.localScale.z, delegate(string s) { _targetTransform.SetZScale(s.ToInt()); });

        _rotationYField.Reinit(_targetTransform.eulerAngles.y, delegate(string s) { _targetTransform.CorrectEulerAngles(y: s.ToInt(), ySetMode: true); });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Delete()
    {
        if (_layer.IsBuildingLayer())
            UI.buildings.Remove(_targetTransform.gameObject);
        else if (_layer.IsEmitterLayer())
            UI.RemoveEmitter(_targetTransform.gameObject);
        else
            Debug.LogError($"Error with {_targetTransform.name} on layer {_layer}");

        Hide();
        Destroy(_targetTransform.gameObject);
    }
    #endregion
    
    #region Private Methods
    #endregion
}
