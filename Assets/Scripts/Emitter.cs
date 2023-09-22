using UnityEngine;

public class Emitter : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    [Range(1000,100_000)]
    private int raysCount = 1000;
    #endregion

    #region Public Fields
    public static int maxReflectionsCount = 20;
    #endregion

    #region Private Fields
    private Vector3[] _rayDirections = null;
    private RayInfo[] _rayInfos = null;
    private Vector3 _position = Vector3.zero;
    private Receiver _receiver = null;
    #endregion


    #region Unity Methods
    private void Start()
    {
        _receiver = FindObjectOfType<Receiver>();
        _receiver.emitters.Add(this);
        
        _rayDirections = Helper.GetUniformPointsOnSphere(raysCount);
        _rayInfos = new RayInfo[raysCount];

        for (int i = 0; i < raysCount; i++)
            _rayInfos[i] = new RayInfo(transform.position, _rayDirections[i]);
    }

    private void OnDestroy()
    {
        _receiver.emitters.Remove(this);
    }

    #endregion
    
    #region Public Methods
    public float GetEmitterSignalPower()
    {
        _position = transform.position;

        Physics.Linecast(_position, _receiver.GetPosition(), out RaycastHit raycastHit);
#if UNITY_EDITOR
        Debug.DrawLine(_position, _receiver.GetPosition(), Color.yellow);
#endif
        
        bool noObstacles = raycastHit.collider?.gameObject.layer.IsReceiverLayer() ?? true;

        if (noObstacles)
            return Helper.GetSignalPower(raycastHit.distance);

        float maxSignalValue = 0.0f;
        
        for (int i = 0; i < raysCount; i++)
        {
            _rayInfos[i].SetPosition(_position);
            maxSignalValue = Mathf.Max(maxSignalValue, _rayInfos[i].GetSignalPowerAtRay());
        }

        return maxSignalValue;
    }
    #endregion
    
    #region Private Methods
    #endregion
}

public class RayInfo
{
    private int   _reflectionsCount;
    private float _distance;

    private Vector3 _position;
    private Vector3 _direction;

    public RayInfo(Vector3 position, Vector3 direction)
    {
        _position  = position;
        _direction = direction;
    }

    public void SetPosition(Vector3 position)
    {
        _position  = position;
    }

    public float GetSignalPowerAtRay()
    {
        _reflectionsCount = 0;
        _distance = 0.0f;
        
        Vector3 temporaryPosition = _position;
        Vector3 temporaryDirection = _direction;

        for (int i = 0; i < Emitter.maxReflectionsCount; i++)
        {
            Ray ray = new Ray(temporaryPosition, temporaryDirection);
            
            bool rayCast = Physics.Raycast(ray, out RaycastHit raycastHit);

            if (!rayCast)
                break;
            
            _distance += raycastHit.distance;
            
#if UNITY_EDITOR
            if (raycastHit.collider.gameObject.layer.IsReceiverLayer())
                DrawRaysInEditor(raycastHit.distance, i);
#endif

            if (raycastHit.collider.gameObject.layer.IsReceiverLayer())
                return Helper.GetSignalPower(_distance, _reflectionsCount);

            _reflectionsCount++;
            
            temporaryPosition = raycastHit.point;
            temporaryDirection = Vector3.Reflect(temporaryDirection, raycastHit.normal);
        }

        return 0.0f;
        
        void DrawRaysInEditor( float distance, int iteration )
        {
            Debug.DrawRay(temporaryPosition, temporaryDirection * (distance == 0.0f ? 1000*0 : distance), GetColor());

            Color GetColor()
            {
                switch (iteration)
                {
                    case 0: return Color.red;
                    case 1: return Color.green;
                    case 2: return Color.blue;
                    case 3: return Color.magenta;
                    
                    default: return Color.black;
                }
            }
        }
    }
}
