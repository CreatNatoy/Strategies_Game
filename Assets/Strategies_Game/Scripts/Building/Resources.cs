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

    private void Start() {
        PrintMoney();
    }

    public void SpendMoney(int spendMoney) {
        _money -= spendMoney;
        PrintMoney();
    }

    public void AddCoin(int coin) {
        _money += coin; 
        PrintMoney();
    }

    private void PrintMoney() => EventManager.PrintCoin(_money); 
}
