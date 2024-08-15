using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "CharacterEvent")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;
    public void Raise(Character character)
    {
        OnEventRaised?.Invoke(character); // if OnEventRaised is not null, invoke it
    }
}
