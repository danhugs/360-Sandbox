using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Broadcaster : MonoBehaviour {
	private Socket s;
	private IPEndPoint ep;
	private IPAddress broadcast;

	private void Start() {
		s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		broadcast = IPAddress.Parse("193.168.1.175");

		ep = new IPEndPoint(broadcast, 11001);
	}

	// Update is called once per frame
	private void Update() {
		byte[] sendbuf = Encoding.ASCII.GetBytes("sending a message via udp babeyyy");

		s.SendTo(sendbuf, ep);

		Debug.Log("Message sent to the broadcast address " + DateTime.Now.ToString());
	}
}