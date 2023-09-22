using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private Text _text = null;
    [SerializeField]
    private CanvasGroup _canvasGroup = null;
    [SerializeField]
    private RectTransform _rectTransform = null;
    #endregion
    
    #region Private Fields
    private float? _showTime = null;
    #endregion
    
    #region Public Fields
    #endregion
    
    
    #region Public Methods
    public void Init( string text, float? showTime = null )
    {
        _text.text = text;
        _showTime = showTime;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        StopAllCoroutines();
        StartCoroutine(ShowTooltip(text));
    }
    #endregion
    
    #region Private Methods
    private IEnumerator ShowTooltip( string text )
    {
        float seconds = _showTime ?? text.Count(x => x == ' ' || x == '\n') + 1;
        
        yield return _canvasGroup.SmoothlySetAlpha(1.0f);
        yield return new WaitForSeconds(seconds);
        yield return _canvasGroup.SmoothlySetAlpha(0.0f);
    }
    #endregion
}
