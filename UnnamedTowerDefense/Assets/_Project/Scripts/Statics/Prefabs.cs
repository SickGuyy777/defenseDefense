using UnityEngine;
using System.Collections.Generic;

public class Prefabs : DontDestroyOnLoad
{
    public static Dictionary<string, GameObject> prefabs =
        new Dictionary<string, GameObject>();

    [SerializeField]
    private Prefab[] _prefabs;

    private void OnValidate()
    {
        prefabs.Clear();
        foreach (var prefab in _prefabs)
        {
            prefabs.Add(prefab.name, prefab.obj);
        }
    }
}

[System.Serializable]
public class Prefab
{
    public string name;
    public GameObject obj;

    public Prefab(string name, GameObject obj)
    {
        this.name = name;
        this.obj = obj;
    }
}