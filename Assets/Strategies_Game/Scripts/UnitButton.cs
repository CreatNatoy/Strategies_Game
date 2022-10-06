using TMPro;
using UnityEngine;

public class UnitButton : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Barack _barack;

    private ServiceLocator _serviceLocator;
    private Resources _resources;
    
    private void Start() {
        _serviceLocator = ServiceLocator.Instance;
        _resources = _serviceLocator.Get<Resources>();

        _priceText.text = _unitPrefab.Price.ToString(); 
    }

    public void TryBuy() {
        var price = _unitPrefab.Price;

        if (_resources.Money < price) return;
        
        _resources.SpendMoney(price);
        _barack.CreateUnit(); 

    }
}
