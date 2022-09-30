using UnityEngine;

public class Mine : Building
{
    [Header("Mine")]
    [SerializeField] private float _periodCoin = 5f;
    [SerializeField] private int _addCoin = 5;

    private float _timer;
    private ServiceLocator _serviceLocator;
    private Resources _resources; 

    public override void Start() {
        base.Start();
        _serviceLocator = ServiceLocator.Instance;
        _resources = _serviceLocator.Get<Resources>();
    }

    private void Update() {
        Timer();
    }

    private void Timer() {
        _timer += Time.deltaTime; 
        if(_timer < _periodCoin) return;

        _timer -= _periodCoin; 
        AddCoin();
    }

    private void AddCoin() => _resources.AddCoin(_addCoin);
}
