using UnityEngine;
using System.Collections;

using System;
using System.IO;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;


public class GetColor : WebSocketService
{
	
	protected override void OnMessage(MessageEventArgs e)
	{
	    Send(e.Data);

		MonoBehaviour.print(e.Data);	

	    TextWriter tw = new StreamWriter("data.txt");
	    tw.WriteLine(e.Data);
   
	    tw.Close();
	}
	
}


public class WebSocketsDemo : MonoBehaviour {
	
	WebSocketServer webbie;
	bool serverStarted = false;
	int serverPort = 4649;
	public string ReceivedColor = "White";
	int fileReadTimer = 0;
	


	void Start () {
		serverStarted = false;
	}
	
	

	void Update () {


		if (fileReadTimer++ > 10) {
	        TextReader tr = new StreamReader("data.txt");
	        ReceivedColor = tr.ReadLine();
	        tr.Close();
			
			fileReadTimer = 0; 
		}
	
		switch (ReceivedColor) {
			case "weiß":
				GetComponent<Renderer>().material.color = Color.white;
				break;
			case "rot":
                GetComponent<Renderer>().material.color = Color.red;
				break;
			case "blau":
                GetComponent<Renderer>().material.color = Color.blue;
				break;
			case "grün":
                GetComponent<Renderer>().material.color = Color.green;
				break;
			case "gelb":
                GetComponent<Renderer>().material.color = Color.yellow;
				break;
			
		}
	}
	
	void OnGUI() {

			if (GUI.Button (new Rect (50,50,80,20), serverStarted?"Stop":"Start" ) ) {
				if (!serverStarted) {
					StartWebSocketServer();
					serverStarted = true;
				} else {
					StopWebSocketServer();
					serverStarted = false;
				}
			}
		
			GUI.Label(new Rect(180, 50,80,25),"Port"); 
			serverPort = System.Convert.ToInt16 (GUI.TextField (new Rect (210, 50, 80,20), serverPort.ToString())) ;
			
			
	}
	
	void StartWebSocketServer() {

      webbie = new WebSocketServer(4649);
	  webbie.AddService<GetColor>("/GetColor");	
				
      webbie.Start();
      print(
        "WebSocket Server listening on port:" + webbie.Port + " service path:");
      foreach (var path in webbie.ServicePaths)
        print("  " + path + "\r\n");
	}
	
	void StopWebSocketServer() {
		webbie.Stop();
	}
	
	
}
