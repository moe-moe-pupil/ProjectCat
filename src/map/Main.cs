using Godot;
using System;


public partial class Main : Node3D
{
	private ENetMultiplayerPeer _peer = new();
	private Godot.Collections.Array<int> _connectedPeerIDs = new();
	private Label _isServerText;
	private Label _peerID;
	private VBoxContainer _menu;
	private int _port = 9999;
	private string _address = "127.0.0.1";
	private Node3D _localPC;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isServerText = GetNode<Label>("NetworkInfo/NetworkSideDisplay");
		_peerID = GetNode<Label>("NetworkInfo/UniquePeerID");
		_menu = GetNode<VBoxContainer>("Menu");
	}

	public void _on_join_pressed()
	{
		_isServerText.Text = "Client";
		_menu.Hide();
		_peer.CreateClient(_address, _port);
		Multiplayer.MultiplayerPeer = _peer;
		_peerID.Text = Multiplayer.GetUniqueId().ToString();
	}

	public void _on_host_pressed()
	{
		_isServerText.Text = "Server";
		_menu.Hide();
		_peer.CreateServer(_port);
		Multiplayer.MultiplayerPeer = _peer;
		_peerID.Text = Multiplayer.GetUniqueId().ToString();
		AddPlayer(1);
		_peer.Connect("peer_connected", new Callable(this, nameof(BindFunc)));
	}

	private async void BindFunc(int newPeerID)
	{
		 await ToSignal(GetTree().CreateTimer(1), "timeout");
		 Rpc("AddNewPC", newPeerID);
		 RpcId(newPeerID, "AddOldPC", _connectedPeerIDs);
		 AddPlayer(newPeerID);
	}

	private void AddPlayer(int peerId)
	{
		_connectedPeerIDs.Add(peerId);
		Node3D pc = GD.Load<PackedScene>("res://src/unit/player.tscn").Instantiate() as Node3D;
		pc.SetMultiplayerAuthority(peerId);
		AddChild(pc);
		if (peerId == Multiplayer.GetUniqueId())
		{
			_localPC = pc;
		}
	}

	[RPC]
	public void AddNewPC(int newPeerID)
	{
		AddPlayer(newPeerID);
	}

	[RPC]
	public void AddOldPC(int[] oldPeerIDs)
	{
		for (int i = 0; i < oldPeerIDs.Length; i++)
		{
			AddPlayer(oldPeerIDs[i]);
		}
	}
}
