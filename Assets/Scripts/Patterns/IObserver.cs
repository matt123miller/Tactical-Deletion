﻿using UnityEngine;
using System.Collections;

public abstract class IObserver : MonoBehaviour {

    public abstract void UpdateThisObserver(WorldState newState);
}
