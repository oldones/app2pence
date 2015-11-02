using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Form : MonoBehaviour {

	[SerializeField]
	InputField _inputField;

	[SerializeField]
	Text _text;

	[SerializeField]
	string _id;

	[SerializeField]
	Button _sendButton;


	public string id{ get { return _id; }}

	bool _hasSent = false;
	string _receivedMsg = "";

	void Update()
	{
		//check if there are new messages
		if(_hasSent && _receivedMsg != "")
		{
			_hasSent = false;
			_text.text = _text.text + "\n" + _receivedMsg;
			_receivedMsg = "";
			_sendButton.interactable = true;
		}
	}

	public void SendMsg()
	{
		_sendButton.interactable = false;
		_hasSent = true;
		WebSocketImpl.instance.SendMsg (this, _inputField.text);
	}

	public void ReceiveMessage(string s)
	{
		_receivedMsg = s;
		_text.text = "a";
	}

	public void ClearMessages()
	{
		_text.text = "";
	}
}
