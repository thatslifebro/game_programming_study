using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController instance;

    public float xmove = 0;
    public float ymove = 0;
    public float distance = 3;
    public GameObject player;
    // Start is called before the first frame update
    private void Awake()
    {
        if (CamController.instance == null)
        {
            CamController.instance = this;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            xmove += Input.GetAxis("Mouse X");
            ymove -= Input.GetAxis("Mouse Y");

            transform.rotation = Quaternion.Euler(ymove, xmove, 0);
            Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance);
            transform.position = player.transform.position - transform.rotation * reverseDistance;
        }
        
    }
}
