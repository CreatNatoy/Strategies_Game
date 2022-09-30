using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolMono<T> where T : MonoBehaviour
{
    private T _prefab;
    private bool _autoExpand;
    private Transform _container;
    private List<T> _pool;

    public PoolMono(T prefab, int count, Transform container, bool autoExpand = false)
    {
        _prefab = prefab;
        _container = container;
        _autoExpand = autoExpand;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();

        for (var i = 0; i < count; i++)
            CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createObject = Object.Instantiate(_prefab, _container);
        createObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createObject);
        return createObject;
    }

    private bool HasFreeElement(out T element)
    {
        element = _pool.FirstOrDefault(p => !p.gameObject.activeInHierarchy);
        element?.gameObject.SetActive(true);
        return element != null;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;

        if (_autoExpand)
            return CreateObject(true);

        throw new Exception($"There is no free elements in pool of {typeof(T)}");
    }

    public List<T> GetAllActiveElements() {
        return _pool.FindAll(p => p.gameObject.activeInHierarchy);
    }
}