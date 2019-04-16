using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }
    Vector3 move;
    RaycastHit hitInfo;
    float force;
    Vector3 downspeed;
    // Update is called once per frame
    void Update()
    {

        move = 7 * Input.GetAxis("Vertical") * transform.forward + 7 * Input.GetAxis("Horizontal") * transform.right;

        transform.position += move * Time.deltaTime;

        downspeed += 9 * Time.deltaTime * Vector3.down;
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, 10, int.MaxValue))
        {
            if (hitInfo.distance < 1.5f)
            {
                downspeed += (1.5f - hitInfo.distance) * 100 * Time.deltaTime * Vector3.up;
                downspeed *= 0.9f;
            }
        }
        transform.position += downspeed * Time.deltaTime;
        downspeed *= 0.98f;
    }
}
