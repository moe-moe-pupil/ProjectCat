// --------------------------------------------------------------------------------------------------------------
// <copyright file="MainUI.cs" company="ProjectCat Technologies and contributors.">
// ��Դ�����ʹ���� GNU AFFERO GENERAL PUBLIC LICENSE version 3 ���֤��Լ��, ���������������ҵ������֤.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Godot;
using GodotUtilities;
using Nakama;
using PlayerStates;

public partial class MainUI : Control
{
    public static readonly string ConfigAddress = "user://uuid.cfg";
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
        var isOK = _uuidConfig.Load(ConfigAddress);
        if (isOK != Error.Ok)
        {
            _uuid = System.Guid.NewGuid().ToString();
            _uuidConfig.SetValue("Player", "uuid", _uuid);
            _uuidConfig.Save(ConfigAddress);
        }
        else
        {
            _uuid = _uuidConfig.GetValue("Player", "uuid").ToString();
            LineEdit name = GetNode<LineEdit>("TabContainer/Login/Menu/UserName");
            name.Text = _uuidConfig.GetValue("Player", "name").ToString();
        }
        this.WireNodes();
    }

    public async void _on_login_button_pressed()
    {
        LineEdit name = GetNode<LineEdit>("TabContainer/Login/Menu/UserName");
        try
        {
            _global.Session = await _global.NakamaClient.AuthenticateDeviceAsync(_uuid, name.Text);
            _global.Socket = Socket.From(_global.NakamaClient);
            await _global.Socket.ConnectAsync(_global.Session, true);
            _global.Socket.ReceivedMatchPresence += presenceEvent =>
            {
                foreach (var presence in presenceEvent.Leaves)
                {
                    _connectedOpponents.Remove(presence);
                    _global.RemovePlayer(presence.Username);
                }
                foreach (var presence in presenceEvent.Joins)
                {
                    _connectedOpponents.Add(presence);
                    _global.AddPlayer(presence.Username);
                }

            };
            var enc = System.Text.Encoding.UTF8;
            _global.Socket.ReceivedMatchState += newState =>
            {
                var content = enc.GetString(newState.State);

                switch (newState.OpCode)
                {
                    case 1:
                        HandlePosAndAnim(newState.UserPresence.Username, content);
                        break;
                    default:
                        break;
                }
            };
            _peerID.Text = _global.Session.Username;
            _uuidConfig.SetValue("Player", "name", _global.Session.Username);
            _uuidConfig.Save(ConfigAddress);

        }
        catch (Nakama.ApiResponseException ex)
        {
            GD.Print(ex);
        }
    }

    public void HandlePosAndAnim(string name, string content)
    {
        Node2D pc = GetNode<CharacterBody2D>(name);
        var basicState = Newtonsoft.Json.JsonConvert.DeserializeObject<SPlayerState>(content);
        pc.Position = basicState.Pos;
        var sprite = pc.GetNode<AnimatedSprite3D>("Sprite");
        sprite.Animation = basicState.Anim;
        sprite.FlipH = basicState.Flip;
    }

    public async void _on_join_pressed()
    {
        try
        {
            _global.Match = await _global.Socket.JoinMatchAsync(_roomName.Text);
            _isServerText.Text = "Client";
            _menu.Hide();
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            GD.Print(_global.Match.Presences.ToString());
            foreach (var presence in _global.Match.Presences)
            {

                _global.AddPlayer(presence.Username);
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex);
        }
    }

    public void _on_restart_pressed()
    {
        GetTree().ReloadCurrentScene();
    }

    public async void _on_host_pressed()
    {
        try
        {
            _global.Match = await _global.Socket.CreateMatchAsync(_roomName.Text);
            _isServerText.Text = _global.Match.Id;
            _menu.Hide();
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            GD.Print(_global.Match.Presences);
            foreach (var presence in _global.Match.Presences)
            {
                _global.AddPlayer(presence.Username);
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex);
        }
    }
}
