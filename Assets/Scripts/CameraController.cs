using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    
    [SerializeField] private float speed;
    [SerializeField] private Vector2 maxXLocations;
    [SerializeField] private Vector2 maxZLocations;

    [SerializeField] private Vector2 minMaxZoom;
    [SerializeField] private float zoomSpeed;
    
    public static CameraController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (GameManager.Instance.isPaused) return;
        
        PanCamera();
        ZoomCamera();
    }

    private void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y >= 1 && _camera.orthographicSize > minMaxZoom.x)
        {
            _camera.orthographicSize -= zoomSpeed;
        }

        if (Input.mouseScrollDelta.y <= -1 && _camera.orthographicSize < minMaxZoom.y)
        {
            _camera.orthographicSize += zoomSpeed;
        }
    }

    private void PanCamera()
    {
        if (Input.GetKey(KeyCode.W) && transform.position.z < maxZLocations.y && transform.position.x < maxXLocations.y)
        {
            float z = transform.position.z + (speed * Time.deltaTime);
            float x = transform.position.x + (speed * Time.deltaTime);
            Vector3 newPosition = new Vector3(x, transform.position.y, z);
            transform.position = newPosition;
        }
        
        if (Input.GetKey(KeyCode.S) && transform.position.z > maxZLocations.x && transform.position.x > maxXLocations.x)
        {
            float z = transform.position.z - (speed * Time.deltaTime);
            float x = transform.position.x - (speed * Time.deltaTime);
            Vector3 newPosition = new Vector3(x, transform.position.y, z);
            transform.position = newPosition;
        }
        
        if (Input.GetKey(KeyCode.A) && transform.position.z < maxZLocations.y && transform.position.x > maxXLocations.x)
        {
            float z = transform.position.z + (speed / 2 * Time.deltaTime);
            float x = transform.position.x - (speed / 2 * Time.deltaTime);
            Vector3 newPosition = new Vector3(x, transform.position.y, z);
            transform.position = newPosition;
        }
        
        if (Input.GetKey(KeyCode.D) && transform.position.z > maxZLocations.x && transform.position.x < maxXLocations.y)
        {
            float z = transform.position.z - (speed / 2 * Time.deltaTime);
            float x = transform.position.x + (speed / 2 * Time.deltaTime);
            Vector3 newPosition = new Vector3(x, transform.position.y, z);
            transform.position = newPosition;
        }
    }

    public void CameraShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake(int duration = 5)
    {
        Vector3 origin = transform.position;
        
        for (int i = 0; i < duration; i++)
        {
            Vector3 destination = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y, transform.position.z + Random.Range(-0.5f, 0.5f));
            transform.position = destination;
            yield return new WaitForSeconds(0.05f);
            transform.position = origin;
        }
    }
}
