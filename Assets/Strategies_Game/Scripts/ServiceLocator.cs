using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[DefaultExecutionOrder(-10)]
public class ServiceLocator : MonoBehaviour
{
    private Dictionary<Type, Object> _itemsMap { get; set; }

    public static ServiceLocator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        _itemsMap = new Dictionary<Type, Object>();
    }

    public void Register(Object newService)
    {
        var type = newService.GetType();
        
        if (_itemsMap.ContainsKey(type))
            throw new Exception($"Cannot add item of type {type}. This type already exists in the service locator");

        _itemsMap[type] = newService;
    }

    public void Unregister(Object service)
    {
        var type = service.GetType();

        if (_itemsMap.ContainsKey(type))
            _itemsMap.Remove(type);
    }

    public TP Get<TP>() where TP : Object
    {
        var type = typeof(TP);

        if (!_itemsMap.ContainsKey(type))
            throw new Exception($"There is no object of type {type} in the service locator");

        return (TP)_itemsMap[type];
    }
}