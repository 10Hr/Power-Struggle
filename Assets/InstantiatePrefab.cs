using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    public GameObject strPrefab;
    public GameObject chaPrefab;
    public GameObject cunPrefab;
    public GameObject intPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(strPrefab);
        Instantiate(chaPrefab);
        Instantiate(cunPrefab);
        Instantiate(intPrefab);
        cunPrefab.GetComponent<SpriteRenderer>().enabled = false;
        chaPrefab.GetComponent<SpriteRenderer>().enabled = false;
        strPrefab.GetComponent<SpriteRenderer>().enabled = false;
        intPrefab.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame 
    void Update()
    {
        
    }
}
