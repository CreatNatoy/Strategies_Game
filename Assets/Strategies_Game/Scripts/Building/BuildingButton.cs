using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private Building _buildingPrefab;

    private ServiceLocator _serviceLocator;
    private Resources _resources; 
    private BuildingPlacer _buildingPlacer;


    private void Start() {
        _serviceLocator = ServiceLocator.Instance;
        _resources = _serviceLocator.Get<Resources>();
        _buildingPlacer = _serviceLocator.Get<BuildingPlacer>(); 
    }

    public void TryBuy() {
        var price = _buildingPrefab.Price;

        if (_resources.Money < price) return;
        
        _resources.SpendMoney(price);
        _buildingPlacer.CreateBuilding(_buildingPrefab);

    }
}
