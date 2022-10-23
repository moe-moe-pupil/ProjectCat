using Godot;
using System;
using Nakama;

public partial class Main : Node3D
{
    private Godot.Collections.Array<int> _connectedPeerIDs = new();
    private Label _isServerText;
    private Label _peerID;
    private VBoxContainer _menu;
    private GlobalScene _global;
    private ConfigFile _uuidConfig = new();
    private string _uuid;
    private ISession _session;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var isOK = _uuidConfig.Load("user://uuid.cfg");
        if (isOK != Error.Ok)
        {
            _uuid = System.Guid.NewGuid().ToString();
            _uuidConfig.SetValue("Player", "uuid", _uuid);
        }
        else
        {
            _uuid = _uuidConfig.GetValue("Player", "uuid").ToString();
        }
        _isServerText = GetNode<Label>("NetworkInfo/NetworkSideDisplay");
        _peerID = GetNode<Label>("NetworkInfo/UniquePeerID");
        _menu = GetNode<VBoxContainer>("Menu");
        _global = GetNode<GlobalScene>("/root/GlobalScene");
    }

    public async void _on_reigister_button_pressed()
    {
        LineEdit name = GetNode<LineEdit>("TabContainer/Register/Menu/UserName");
        try
        {
            _session = await _global.NakamaClient.AuthenticateDeviceAsync(_uuid, name.Text);
        }
        catch (Nakama.ApiResponseException ex)
        {
            GD.Print(ex);
        }
    }

    public void _on_join_pressed()
    {
        _isServerText.Text = "Client";
        _menu.Hide();
        _peerID.Text = "";
    }

    public void _on_restart_pressed()
    {
        GetTree().ReloadCurrentScene();
    }

    public void _on_host_pressed()
    {
        _isServerText.Text = "Server";
        _menu.Hide();
        _peerID.Text = Multiplayer.GetUniqueId().ToString();
        AddPlayer(1);
    }

    private async void BindFunc(int newPeerID)
    {
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        Rpc("AddNewPC", newPeerID);
        RpcId(newPeerID, "AddOldPC", _connectedPeerIDs);
        AddPlayer(newPeerID);
        GD.Print(newPeerID);
    }

    private void AddPlayer(int peerId)
    {
        _connectedPeerIDs.Add(peerId);
        Node3D pc = GD.Load<PackedScene>("res://src/unit/player.tscn").Instantiate() as Node3D;
        pc.SetMultiplayerAuthority(peerId);
        AddChild(pc);
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
