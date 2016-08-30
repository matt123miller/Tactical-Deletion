using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class IInventory : MonoBehaviour
{
    [SerializeField]
    private List<KeyCard> keycards = new List<KeyCard>();

    public List<KeyCard> Keycards
    {
        get { return keycards; }
        set { keycards = value; }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
