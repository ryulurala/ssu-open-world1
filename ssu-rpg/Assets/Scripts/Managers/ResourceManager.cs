﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instaniate(string name, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{name}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab: {name}");
            return null;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;    // (Clone) 없애기

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}