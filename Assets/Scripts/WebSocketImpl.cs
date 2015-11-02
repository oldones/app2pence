using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;

public class WebSocketImpl : MonoBehaviour {

	public static WebSocketImpl instance{ get; private set; }

	WebSocket _ws;

	//Bonus task: this dictionary will store the information from the various senders.
	Dictionary< string, Form > _senders;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		_senders = new Dictionary<string, Form> ();
		InitWebSocket ();
	}

	void InitWebSocket()
	{
		_ws = new WebSocket ("wss://echo.websocket.org/");
		_ws.OnMessage += MessageCallback;
		_ws.OnError += ErrorCallback;
		_ws.Connect ();
	}

	//send message to the server
	public void SendMsg(Form sender, string msg)
	{
		//avoid overriding messages
		if(!_senders.ContainsKey(sender.id))
		{
			_senders.Add(sender.id, sender);
			//attach the sender id so we know who sent the message
			msg = sender.id + ";" + msg;
			_ws.Send (msg);
		}
	}

	//method that handles messages received from the server
	void MessageCallback(object sender, MessageEventArgs e)
	{
		string[] split = e.Data.Split (';');
		Form f = _senders [split [0]];

		//update the scroll rect of the sender
		f.ReceiveMessage (split [1]);

		//remove the sender from the dictionary of senders expecting messages
		_senders.Remove (split [0]);
	}

	//method that handles error messages
	void ErrorCallback(object sender, ErrorEventArgs e)
	{
		Debug.Log (e.Message);
	}
}
