using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu( menuName = "Event/VoidEnentSO")]

public class VoidEnentSO :ScriptableObject
{
    public UnityAction OnEnentRaised;

    public void RaiseEvent()
    {
        OnEnentRaised?.Invoke();
    }
}