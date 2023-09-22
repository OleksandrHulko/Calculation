using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using OptionData = UnityEngine.UI.Dropdown.OptionData;

public class UI : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private ObjectInfoWindow _objectInfoWindow = null;
    [SerializeField]
    private Tooltip _tooltip = null;
    [SerializeField]
    private Dropdown _dropdown = null;
    [SerializeField]
    private Transform _firstTransform = null;
    [SerializeField]
    private RectTransform _emitterButtonRT = null;
    [SerializeField]
    private RectTransform _receiverButtonRT = null;
    [SerializeField]
    private Camera _camera = null;
    [SerializeField]
    private GameObject _cube = null;
    [SerializeField]
    private GameObject _emitter = null;
    [SerializeField]
    private Receiver _receiver = null;
    #endregion

    #region Public Fields
    public static List<GameObject> buildings = new List<GameObject>();
    #endregion
    
    #region Private Fields
    private static List<GameObject> _emitters = new List<GameObject>();
    private static List<RectTransform> _emittersBtnRT = new List<RectTransform>();
    private bool _mouse1Pressed = false;
    #endregion


    #region Unity Methods
    private void Start()
    {
        _objectInfoWindow.Init();
        InitDropdown();
        AddNewEmitter();
        _receiver.transform.position = Vector3.up * 30;
    }

    private void Update()
    {
        ReadInput();
        TryOpenObjectInfoWindow();
        ShowFlyButtonsOnScreen();
    }

    #endregion
    
    #region Private Methods
    private void ReadInput()
    {
        _mouse1Pressed = Input.GetKeyDown(KeyCode.Mouse1);
    }

    private void InitDropdown()
    {
        InitDropdownOptions();
        _dropdown.onValueChanged.AddListener(SpawnObjectsFromSaving);
    }

    private void ReinitDropdown()
    {
        _dropdown.ClearOptions();
        InitDropdownOptions();
    }

    private void InitDropdownOptions()
    {
        _dropdown.options.Add(new OptionData("Empty"));
        
        foreach (LevelInfo levelInfo in SaveManager.levelInfos)
            _dropdown.options.Add(new OptionData(levelInfo.name));
        
        _dropdown.RefreshShownValue();
    }

    private void TryOpenObjectInfoWindow()
    {
        if(!_mouse1Pressed)
            return;
        
        _objectInfoWindow.Hide();
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(ray, out RaycastHit raycastHit))
            return;

        int layer = raycastHit.collider.gameObject.layer;
        
        if(layer.IsGroundLayer())
            return;
        
        _objectInfoWindow.Reinit(raycastHit.transform );
    }

    private void SpawnObjectsFromSaving( int number )
    {
        foreach (GameObject building in buildings) 
            Destroy(building);

        foreach (GameObject emitter in _emitters) Destroy(emitter);

        if (number == 0)
        {
            buildings = new List<GameObject>(0);
            DestroyAllEmitters();
            AddNewEmitter();
            _receiver.transform.position = Vector3.up * 30;
            return;
        }

        int index = number - 1;
        
        List<ObjectInfo> buildingsInfo = SaveManager.levelInfos[index].buildingsInfo;
        List<ObjectInfo> emittersInfo  = SaveManager.levelInfos[index].emittersInfo;
        Vector3 receiverPosition       = SaveManager.levelInfos[index].receiverInfo.GetPosition();

        buildings = new List<GameObject>(buildingsInfo.Count);
        DestroyAllEmitters();

        foreach (ObjectInfo buildingInfo in buildingsInfo)
        {
            GameObject tempGameObject = Instantiate(_cube);
            tempGameObject.transform.ChangeTransform(buildingInfo);

            buildings.Add(tempGameObject);
        }

        foreach (ObjectInfo emitterInfo in emittersInfo) 
            AddNewEmitter(emitterInfo.GetPosition());

        _receiver.transform.position = receiverPosition;
    }

    private void ShowFlyButtonsOnScreen()
    {
        _receiverButtonRT.position = _camera.WorldToScreenPoint(_receiver.transform.position);

        for (int i = 0; i < _emitters.Count; i++)
            _emittersBtnRT[i].position = _camera.WorldToScreenPoint(_emitters[i].transform.position);
    }

    private void AddNewEmitter(Vector3? position = null, bool showInfoWindow = false)
    {
        GameObject tempGameObject = Instantiate(_emitter);
        tempGameObject.transform.position = position ?? Vector3.up * 40;

        RectTransform tempRectTransform = Instantiate(_emitterButtonRT, _firstTransform);
        _emittersBtnRT.Add(tempRectTransform);

        _emitters.Add(tempGameObject);

        if (showInfoWindow)
            _objectInfoWindow.Reinit(tempGameObject.transform);
    }
    #endregion

    #region Public Methods
    public void GenerateBuildings()
    {
        _dropdown.value = 0;
        
        int buildingsCount = Random.Range(4, 10);

        foreach (GameObject building in buildings) 
            Destroy(building);

        buildings = new List<GameObject>(buildingsCount);

        for (int i = 0; i < buildingsCount; i++)
        {
            GameObject tempGameObject = Instantiate(_cube, GetRandomPosition(), GetRandomYRotation());
            tempGameObject.transform.localScale = GetRandomSize();
            
            buildings.Add(tempGameObject);
        }
        

        Vector3 GetRandomPosition()
        {
            int range = 45;
            int x = Random.Range(-range,range);
            int z = Random.Range(-range,range);

            return new Vector3(x, 0, z);
        }

        Vector3 GetRandomSize()
        {
            int x = Random.Range(7, 30);
            int y = Random.Range(5, 30);
            int z = x >= 15 ? x / 2 : Random.Range(7, 30);

            return new Vector3(x, y, z);
        }

        Quaternion GetRandomYRotation()
        {
            int random = Random.Range(-45, 45);
            int y = random % 2 == 0 ? random : 0;
            
            return Quaternion.Euler(0.0f, y, 0.0f);
        }
    }

    public void Save()
    {
        string name = DateTime.Now.ToString();
        
        List<ObjectInfo> buildingsInfo = buildings.Select(x => (ObjectInfo)x.transform).ToList();
        List<ObjectInfo> emittersInfos = _emitters.Select(x => (ObjectInfo)x.transform).ToList();

        SaveManager.levelInfos.Add(new LevelInfo(buildingsInfo, emittersInfos, _receiver.transform, name));

        _tooltip.Init($"Saved {name}", 1.0f);
        
        ReinitDropdown();
    }

    public void OnAddNewBuildingClick()
    {
        GameObject tempGameObject = Instantiate(_cube);
        tempGameObject.transform.localScale = Vector3.one * 10;
        
        buildings.Add(tempGameObject);
        
        _objectInfoWindow.Reinit(tempGameObject.transform);
    }

    public void OnAddNewEmitterClick()
    {
        AddNewEmitter(showInfoWindow: true);
    }

    public void DestroyAllEmitters()
    {
        while (_emitters.Count > 0)
        {
            GameObject emitter = _emitters[0];
            RemoveEmitter(emitter);
            Destroy(emitter);
        }
    }

    public static void RemoveEmitter( GameObject emitter )
    {
        _emitters.Remove(emitter);

        RectTransform tempRectTransform = _emittersBtnRT[0];
        _emittersBtnRT.Remove(tempRectTransform);
        Destroy(tempRectTransform.gameObject);
    }

    public void OnEmitterClick(RectTransform rectTransform)
    {
        int index = _emittersBtnRT.IndexOf(rectTransform);

        _objectInfoWindow.Reinit(_emitters[index].transform);
    }

    public void OnReceiverClick()
    {
        _objectInfoWindow.Reinit(_receiver.transform);
    }

    public void OnCalculateClick()
    {
        _tooltip.Init($"Please wait");
        float signalValue = Mathf.Clamp(_receiver.GetSignalValue() * 100, 0.0f, 100.0f);
        signalValue = (float)Math.Round(signalValue, 3);
        string text = signalValue > 0.0f ? $"Signal power - {signalValue}%" : "No signal";
        _tooltip.Init(text, 1.0f);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
