using Godot;
using System;
using Nakama;
using System.Collections.Generic;
using Newtonsoft;
using GodotUtilities;

public partial class MainUI : Control
{
    static readonly string ConfigAddress = "user://uuid.cfg";
    private List<IUserPresence> _connectedOpponents = new(2);
    [Node("NetworkInfo/NetworkSideDisplay")]
    private Label _isServerText;
    [Node("NetworkInfo/UniquePeerID")]
    private Label _peerID;
    [Node("TabContainer")]
    private TabContainer _menu;
    [Node("/root/GlobalScene")]
    private GlobalScene _global;
    private ConfigFile _uuidConfig = new();
    [Node("TabContainer/Net/Menu/RoomName")]
    private LineEdit _roomName;
    private string _uuid;
    // Called when the node enters the scene tree for the first time.

    /// <inheritdoc/>
    public override void _Ready()
    {
        var isOK = this._uuidConfig.Load(ConfigAddress);
        if (isOK != Error.Ok)
        {
            this._uuid = System.Guid.NewGuid().ToString();
            this._uuidConfig.SetValue("Player", "uuid", this._uuid);
            this._uuidConfig.Save(ConfigAddress);
        }
        else
        {
            this._uuid = this._uuidConfig.GetValue("Player", "uuid").ToString();
            LineEdit name = this.GetNode<LineEdit>("TabContainer/Login/Menu/UserName");
            name.Text = this._uuidConfig.GetValue("Player", "name").ToString();
        }
        this.WireNodes();
    }

    public async void _on_login_button_pressed()
    {
        LineEdit name = this.GetNode<LineEdit>("TabContainer/Login/Menu/UserName");
        try
        {
            this._global.Session = await this._global.NakamaClient.AuthenticateDeviceAsync(this._uuid, name.Text);
            this._global.Socket = Socket.From(this._global.NakamaClient);
            await this._global.Socket.ConnectAsync(this._global.Session, true);
            this._global.Socket.ReceivedMatchPresence += presenceEvent =>
            {
                foreach (var presence in presenceEvent.Leaves)
                {
                    this._connectedOpponents.Remove(presence);
                    this._global.RemovePlayer(presence.Username);
                }
                foreach (var presence in presenceEvent.Joins)
                {
                    this._connectedOpponents.Add(presence);
                    this._global.AddPlayer(presence.Username);
                }

            };
            var enc = System.Text.Encoding.UTF8;
            this._global.Socket.ReceivedMatchState += newState =>
            {
                var content = enc.GetString(newState.State);

                switch (newState.OpCode)
                {
                    case 1:
                        this.HandlePosAndAnim(newState.UserPresence.Username, content);
                        break;
                    default:
                        break;
                }
            };
            this._peerID.Text = this._global.Session.Username;
            this._uuidConfig.SetValue("Player", "name", this._global.Session.Username);
            this._uuidConfig.Save(ConfigAddress);

        }
        catch (Nakama.ApiResponseException ex)
        {
            GD.Print(ex);
        }
    }
    public void HandlePosAndAnim(string name, string content)
    {
        Node2D pc = this.GetNode<CharacterBody2D>(name);
        var basicState = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerState.BasicState>(content);
        pc.Position = basicState.Pos;
        var sprite = pc.GetNode<AnimatedSprite3D>("Sprite");
        sprite.Animation = basicState.Anim;
        sprite.FlipH = basicState.Flip;
    }

    public async void _on_join_pressed()
    {
        try
        {
            this._global.Match = await this._global.Socket.JoinMatchAsync(this._roomName.Text);
            this._isServerText.Text = "Client";
            this._menu.Hide();
            await this.ToSignal(this.GetTree().CreateTimer(1), "timeout");
            GD.Print(this._global.Match.Presences.ToString());
            foreach (var presence in this._global.Match.Presences)
            {

                this._global.AddPlayer(presence.Username);
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex);
        }
    }

    public void _on_restart_pressed()
    {
        this.GetTree().ReloadCurrentScene();
    }

    public async void _on_host_pressed()
    {
        try
        {
            this._global.Match = await this._global.Socket.CreateMatchAsync(this._roomName.Text);
            this._isServerText.Text = this._global.Match.Id;
            this._menu.Hide();
            await this.ToSignal(this.GetTree().CreateTimer(1), "timeout");
            GD.Print(this._global.Match.Presences);
            foreach (var presence in this._global.Match.Presences)
            {
                this._global.AddPlayer(presence.Username);
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex);
        }
    }
}
