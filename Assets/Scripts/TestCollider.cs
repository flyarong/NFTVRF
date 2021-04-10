using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> obstacles;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        obstacles.Add(other.name);
    }
    private void OnTriggerExit(Collider other)
    {
        obstacles.Remove(other.name);
    }
}
