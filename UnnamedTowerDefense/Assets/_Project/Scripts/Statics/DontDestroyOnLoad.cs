using UnityEngine;

[DefaultExecutionOrder(-int.MaxValue)]
public class DontDestroyOnLoad : MonoBehaviour
{
    protected virtual void OnAwake() { }
    private void Awake()
    {
        Object.DontDestroyOnLoad(this);
        OnAwake();
    }
}