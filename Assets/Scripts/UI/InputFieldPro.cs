using System;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldPro : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private Text _titleText = null;
    [SerializeField]
    private Button _buttonPlus = null;
    [SerializeField]
    private Button _buttonMinus = null;
    [SerializeField]
    private InputField _inputField = null;
    #endregion

    #region Private Fields
    private Action<string> _onValueChanged = null;
    private int _value = 0;
    #endregion


    #region Public Methods
    public void Init(string title)
    {
        _titleText.text = title;
        
        _buttonPlus .onClick.AddListener(OnButtonPlusClickHandler);
        _buttonMinus.onClick.AddListener(OnButtonMinusClickHandler);
        
        _inputField.onValueChanged.AddListener(OnValueChangedHandler);
        _inputField.onEndEdit     .AddListener(OnEndEdit);
    }

    public void Reinit(float value, Action<string> onValueChanged)
    {
        _onValueChanged = onValueChanged;
        _value = Mathf.RoundToInt(value);
        
        _inputField.text = _value.ToString();
    }
    #endregion
    
    #region Private Methods
    private void OnButtonPlusClickHandler()
    {
        string valueStr = (++_value).ToString();
        
        _onValueChanged.Invoke(valueStr);
        _inputField.text = valueStr;
    }
    
    private void OnButtonMinusClickHandler()
    {
        string valueStr = (--_value).ToString();
        
        _onValueChanged.Invoke(valueStr);
        _inputField.text = valueStr;
    }

    private void OnValueChangedHandler( string value )
    {
        _value = value.ToInt();
        _onValueChanged.Invoke(value);
    }

    private void OnEndEdit( string value )
    {
        if (value == string.Empty || value == "-")
            _inputField.text = 0.ToString();
    }
    #endregion
}
