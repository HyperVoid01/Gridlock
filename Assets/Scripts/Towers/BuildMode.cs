using System.Collections;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 mouseWorldPos;
    public float groundLevel = 0.5f;
    public GameObject buildBox;
    public LayerMask groundLayer;

    public bool towerPlaced = false;
    public bool positioning = false;
    private GameObject _currentTower;

    public Vector2 xBuildSize;
    public Vector2 zBuildSize;
    
    public static BuildMode Instance;
    
    public Tower currentTower = null;
    public Generator currentGenerator = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void Update()
    {
        if (GameManager.Instance.isPaused) return;
        
        if (positioning)
            PositionTower();
    }

    public void PlaceTower(GameObject tower)
    {
        if (tower.GetComponent<Tower>())
            currentTower = tower.GetComponent<Tower>();
        else if (tower.GetComponent<Generator>())
            currentGenerator = tower.GetComponent<Generator>();
        else
            return;
        
        // Checks if on build mode and if already placed tower
        if (!RoundManager.Instance.onBuildStage || towerPlaced)
        {
            currentTower = null;
            currentGenerator = null;
            return;
        }

        if (!currentTower && currentGenerator)
        {
            // Generator Checks
            if (GameManager.Instance.energy < currentGenerator.energyCost)
            {
                currentTower = null;
                currentGenerator = null;
                return;
            }
        }
        else if (currentTower && !currentGenerator)
        {
            // Tower Checks
            if (!GameManager.Instance.CheckTowerAvailability(currentTower.energyType))
            {
                currentTower = null;
                currentGenerator = null;
                return;
            }
        }
        
        _currentTower = tower.gameObject;
        positioning = true;
        buildBox.SetActive(true);
    }

    private void PositionTower()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            mouseWorldPos = hit.point;
            
            int groundMask = 1 << LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(mouseWorldPos + Vector3.up, Vector3.down, out var groundHit, 5f, groundMask))
            {
                mouseWorldPos.y = groundHit.point.y + groundLevel;
            }
            else
            {
                mouseWorldPos.y = groundLevel;
            }
        }
        else
        {
            mouseWorldPos.y = groundLevel;
        }

        if (mouseWorldPos.x > xBuildSize.x && mouseWorldPos.x < xBuildSize.y &&
            mouseWorldPos.z > zBuildSize.x && mouseWorldPos.z < zBuildSize.y)
        {
            buildBox.transform.position = mouseWorldPos;
        }
        
        if (Input.GetMouseButton(0))
        {
            if (currentGenerator && !currentTower)
            {
                GameManager.Instance.energy -= currentGenerator.energyCost;
            }
            
            UIManager.Instance.UpdateEnergy(GameManager.Instance.energy);
            buildBox.SetActive(false);
            towerPlaced = true;
            positioning = false;
            GameObject placedTower = Instantiate(_currentTower, buildBox.transform.position, Quaternion.identity);
            
            if (currentTower && !currentGenerator)
                GameManager.Instance.AddToTowerCount(placedTower.GetComponent<Tower>());
            else if (currentGenerator && !currentTower)
                GameManager.Instance.AddToGeneratorCount(placedTower.GetComponent<Generator>());
            
            StartCoroutine(Wait());
            
            currentTower = null;
            currentGenerator = null;
        }


    }

    public void ExitBuildMode()
    {
        UIManager.Instance.HideShopHUD();
        buildBox.SetActive(false);
        towerPlaced = false;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        
        towerPlaced = false;
    }
}
