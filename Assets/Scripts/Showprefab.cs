using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPrefaf : MonoBehaviour
{
    private GameObject variableForPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Assets / Models / Characters / Blue_Knight.prefab
        variableForPrefab = (GameObject)Resources.Load("Models/Characters/Blue_Knight", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
