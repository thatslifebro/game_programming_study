using System;
using System.Collections;
using System.Collections.Generic;
using ServerCore;
using UnityEngine;
using static S_PlayerList;

public class MyPlayer : Player
{
    NetworkManager _network;
    // Start is called before the first frame update
    public GameObject Cam;
    public float Speed;

    private Vector3 MoveDir;

    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        Cam = GameObject.Find("Main Camera").gameObject;

        //StartCoroutine("CoSendPacket");
        CamController.instance.player = this.gameObject;
        
        Speed = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            var offset = Cam.transform.forward;
            offset.y = 0;
            transform.LookAt(transform.position + offset);
        }

        MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        MoveDir = transform.TransformDirection(MoveDir);
        MoveDir *= Speed;

        Vector3 newPosition = transform.position + MoveDir * Time.deltaTime;
        transform.position = newPosition;

    }

    IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            


            // wasd 누를때만 보는 방향 교체
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                var offset = Cam.transform.forward;
                offset.y = 0;
                transform.LookAt(transform.position + offset);
            }

            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            MoveDir = transform.TransformDirection(MoveDir);
            MoveDir *= Speed;

            Vector3 newPosition = transform.position + MoveDir * 0.1f;

            C_Move movePacket = new C_Move();
            movePacket.posX = newPosition.x;
            movePacket.posY = 0;
            movePacket.posZ = newPosition.z;

            _network.Send(movePacket.Serialize());
        }
    }
}
