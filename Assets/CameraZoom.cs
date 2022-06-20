using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float _fov;

    [SerializeField]
    [Tooltip("Sensitivity of how aggressive the zoom will be")]
    private float _sensitivity;

    private const int defaultFov = 60;
    private const int _minFov = 40;
    private const int _maxFov = 60;    

    // Start is called before the first frame update
    void Start()
    {
        _fov = defaultFov; 
    }

    // Update is called once per frame
    void Update()
    {
        _fov -= Input.GetAxis("Mouse ScrollWheel") * _sensitivity;
        _fov = Mathf.Clamp(_fov, _minFov, _maxFov);
        Camera.main.fieldOfView = _fov;
    }
}
