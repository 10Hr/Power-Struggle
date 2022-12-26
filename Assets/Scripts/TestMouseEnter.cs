using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TestMouseEnter : NetworkBehaviour
{
    [SerializeField]
    public bool hovered;
    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnMouseEnter()
    {
        hovered = true;
        rend.color = Color.red;
    }

    public void OnMouseExit()
    {
        hovered = false;
        rend.color = Color.white;
    }
}
