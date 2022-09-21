using UnityEngine;

public class Resources : MonoBehaviour
{
    [SerializeField] private int _money = 100;

    private ServiceLocator _serviceLocator; 
    
    public int Money => _money;

    private void Awake() {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.Register(this); 
    }

    public void SpendMoney(int spendMoney) => _money -= spendMoney;
}
