using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    public PlayerControls playerControlers;
    void Start()
    {
        /*if (playerControlers.facingRight)
        {
            transform.position = new Vector3(player.transform.position.x + 1, 0, -10);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x - 1, 0, -10);
        }*/
        transform.position = new Vector3(player.transform.position.x, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playerControlers.facingRight)
        {
            transform.position = new Vector3(player.transform.position.x + 1, 0, -10);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x - 1, 0, -10);
        }*/
        transform.position = new Vector3(player.transform.position.x, 0, -10);
    }
}
