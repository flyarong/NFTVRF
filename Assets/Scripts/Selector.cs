using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public Material NewMatarial;
    public GameObject player;
    public float speed = 1.0f;
    private Transform target;
    private bool stoped = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit; 
            if (Physics.Raycast(ray, out hit)) {
                var select = hit.transform;
            
                var selectionRender = select.GetComponent<Renderer>();
                if (hit.collider.tag == "Ground")
                {
                    var mylist = player.GetComponent<TestCollider>();
                    foreach (var item in mylist.obstacles)
                    {
                        if (item == select.gameObject.name)
                        {
                            if (stoped)
                            {
                                target = select.transform;
                                print(select.gameObject.name);
                                stoped = false;
                                player.GetComponent<Animator>().SetBool("running", true);

                            }
                        }
                    }
                }              
            }
        }
        if (!stoped)
        {
            var playerPosition = new Vector3(player.transform.position.x, 1.05f, player.transform.position.z);
            var targePosition = new Vector3(target.transform.position.x, 1.05f, target.transform.position.z);
            Vector3 direction = targePosition - playerPosition;
            Quaternion rotation = Quaternion.LookRotation(direction);
            player.transform.rotation = rotation;
            transform.LookAt(target);
            player.transform.position = Vector3.MoveTowards(playerPosition, targePosition, step);
            if (targePosition == playerPosition)
            {
                player.GetComponent<Animator>().SetBool("running", false);
                stoped = true;
            }
        }
    }
}
