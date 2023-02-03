// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
//using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Network;
using Mirror;
using Unity.Services.Relay.Models;
using UnityEngine.SceneManagement;
using TMPro;


namespace UI
{
	/// <summary>
	/// An extension for the NetworkManager that displays a default HUD for controlling the network state of the game.
	/// <para>This component also shows useful internal state for the networking system in the inspector window of the editor. It allows users to view connections, networked objects, message handlers, and packet statistics. This information can be helpful when debugging networked games.</para>
	/// </summary>
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/MenuUI")]
	[RequireComponent(typeof(MyNetworkManager))]
	//[EditorBrowsable(EditorBrowsableState.Never)]
	[HelpURL("https://mirror-networking.com/docs/Components/NetworkManagerHUD.html")]
	public class MenuUI : MonoBehaviour
	{
		[SerializeField]
		private MyNetworkManager m_Manager;
		
		public GameObject joinCodeLbl;
		public GameObject joinCodeInput;
		public GameObject waitingforplayers;

		[SerializeField]
		public PlayerList playerList;

		public GameObject maincanvas;
		public GameObject menucanvas;

		/// <summary>
		/// Whether to show the default control HUD at runtime.
		/// </summary>
		public bool showGUI = true;

		/// <summary>
		/// The horizontal offset in pixels to draw the HUD runtime GUI at.
		/// </summary>
		public int offsetX;

		/// <summary>
		/// The vertical offset in pixels to draw the HUD runtime GUI at.
		/// </summary>
		public int offsetY;

		void Awake()
		{

			//m_Manager = GetComponent<MyNetworkManager>();

			maincanvas = GameObject.Find("Canvas");
			menucanvas = GameObject.Find("MenuCanvas");
			//maincanvas.SetActive(false);

			joinCodeLbl = GameObject.Find("joinCodeLbl");
			joinCodeInput = GameObject.Find("joinCodeInput");
			//playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();

		}

		void OnGUI()
		{
			string[] args = System.Environment.GetCommandLineArgs();
			foreach (string arg in args)
			{
				if (arg == "-server")
				{
					m_Manager.StartServer();
					showGUI = false;
				}
			}

			if (!showGUI)
				return;

			if (!NetworkClient.isConnected && !NetworkServer.active) { 
			} else {
				StatusLabels();
			}

			// client ready
			if (NetworkClient.isConnected && !NetworkClient.ready)
			{
				NetworkClient.Ready();

				if (NetworkClient.localPlayer == null)
				{
					NetworkClient.AddPlayer();
				}

			}

			//StopButtons();

		}
		public void CopyToClipboard()
		{
			TextEditor te = new TextEditor();
			if (m_Manager.IsRelayEnabled())
				te.text = m_Manager.relayJoinCode;
			else 
				te.text = "No relay join code available";
			
			te.SelectAll();
			te.Copy();
		}

		public void AuthenticatePlayer() {
			Debug.Log("logged in!");
			if (!m_Manager.isLoggedIn)
				m_Manager.UnityLogin();
			
		}

		public void HostGame() {

			
			if (!NetworkClient.active)
			{
				if (m_Manager.isLoggedIn)
				{
					// Server + Client
					if (Application.platform != RuntimePlatform.WebGLPlayer)
					{
						int maxPlayers = 4;
						m_Manager.StartRelayHost(maxPlayers);
						//playerList.GetComponent<PlayerList>().CmdAddPlayers(NetworkClient.localPlayer.GetComponent<PlayerScript>());
						
					}
				} else
					m_Manager.UnityLogin(); // add log in button
			}


		}

		public void JoinGame() {

			try {
					joinCodeInput = GameObject.Find("joinCodeInput");
					waitingforplayers = GameObject.Find("waitingforplayers");
					m_Manager.relayJoinCode = joinCodeInput.GetComponent<TMP_InputField>().text;
					m_Manager.JoinRelayServer();
					waitingforplayers.GetComponent<TMP_Text>().text = "Waiting for players...";

			} catch (InvalidOperationException e) {
				Debug.Log("No Relay server found with code: " + joinCodeInput.GetComponent<TMP_InputField>().text + " " + e);
			}

		}

		public void QuitGame() {
			Application.Quit();
		}
		

		public void StartGame() {
			//Debug.Log(playerList.GetComponent<PlayerList>().players.Count);
			if (playerList.players.Count == 4) {
				Debug.Log("Starting game RELAY STYLE!!!");

			}

			
		}
		// does nothing delete after finished
		void StartButtons()
		{
			if (!NetworkClient.active)
			{
				if (m_Manager.isLoggedIn)
				{
					// Server + Client
					if (Application.platform != RuntimePlatform.WebGLPlayer)
					{
						GUILayout.BeginHorizontal();
						if (GUILayout.Button("Relay Host"))
						{
							int maxPlayers = 4;
							m_Manager.StartRelayHost(maxPlayers);

						}
						GUILayout.EndHorizontal();
					}



					// Client + Relay Join Code
					GUILayout.BeginHorizontal();
					if (GUILayout.Button("Client (with Relay)"))
					{
						m_Manager.JoinRelayServer();
					}
					m_Manager.relayJoinCode = GUILayout.TextField(m_Manager.relayJoinCode);
					GUILayout.EndHorizontal();

				}
				else
				{
					if (GUILayout.Button("Auth Login"))
					{
						m_Manager.UnityLogin();
					}
				}
			}
			else
			{
				// Connecting
				GUILayout.Label("Connecting to " + m_Manager.networkAddress + "..");
				if (GUILayout.Button("Cancel Connection Attempt"))
				{
					m_Manager.StopClient();
				}
			}
		}

		void StatusLabels()
		{
			// server / client status message
			if (NetworkServer.active)
			{
				if (m_Manager.IsRelayEnabled() ) //?  
				{
					joinCodeLbl = GameObject.Find("joinCodeLbl");
					if (joinCodeLbl != null)
						joinCodeLbl.GetComponent<TextMeshProUGUI>().text = "JOIN CODE: " + m_Manager.relayJoinCode; // not working?
					
				}
					
			}

		}

		void StopButtons()
		{
			// stop host if host mode
			if (NetworkServer.active && NetworkClient.isConnected)
			{
				if (GUILayout.Button("Stop Host"))
				{
					m_Manager.StopHost();
				}
			}
			// stop client if client-only
			else if (NetworkClient.isConnected)
			{
				if (GUILayout.Button("Stop Client"))
				{
					m_Manager.StopClient();
				}
			}
			// stop server if server-only
			else if (NetworkServer.active)
			{
				if (GUILayout.Button("Stop Server"))
				{
					m_Manager.StopServer();
				}
			}
		}
	}
}
