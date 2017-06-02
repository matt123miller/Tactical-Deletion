using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public interface ISubject
{
    
    void Subscribe(IObserver observer);
    void Unsubscribe(IObserver observer);
    void Notify(WorldState newState);
}
