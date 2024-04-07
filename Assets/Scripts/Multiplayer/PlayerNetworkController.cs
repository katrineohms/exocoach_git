using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Udp;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    public UdpHost udpHost;
    public ElbowPositioner elbow;

    public static event Action<float> OnBorderCrossed;

    [Networked, Capacity(64)] public string Message { get; set; }
    [Networked(OnChanged = nameof(SendHost))] public NetworkBool HostUnchanged { get; set; }
    [Networked(OnChanged = nameof(SendUpper))] public float UpperBorder { get; set; }
    [Networked(OnChanged = nameof(SendLower))] public float LowerBorder { get; set; }
    [Networked(OnChanged = nameof(EnableBorders))] public NetworkBool BordersEnabled { get; set; }
    [Networked] public NetworkBool AlertEnabled { get; set; }
    [Networked(OnChanged = nameof(SendMirrorMessage)), Capacity(32)] public string MirrorMessage { get; set; }
    [Networked(OnChanged = nameof(SendReleaseMessage))] public NetworkBool ReleaseControl { get; set; }
    [Networked(OnChanged = nameof(StartedBorders))] public NetworkBool BorderExperiment { get; set; }

    private PlayerController playerController;
    private NetworkObject network;
    private bool borderCrossed;

    // Start is called before the first frame update
    void Start()
    {
        //Ensure same format for floats across systems
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        playerController = FindObjectOfType<PlayerController>();
        elbow = playerController.GetElbowPositioner();
        UdpHost.OnReceiveMsg += OnMotorValue;
        RecordController.OnResendMsg += OnMotorValue;
        HostUnchanged = true;
        network = GetComponent<NetworkObject>();
        //Only for testing
        Simulator.OnDoSimulation += OnMotorValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Runner != null)
        {
            if (!IsMaster() && !ReleaseControl)
                udpHost.SendMsg(Message);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner != null)
        {
            if (!IsMaster() && Runner.IsServer)
            {
                string[] values = Message.Split(',');
                float elbowValue = float.Parse(values[1]);
                elbow.SetElbow(elbowValue, float.Parse(values[2]));
                BorderCrossCheck(elbowValue);
            }
        }
    }

    private void OnMotorValue(string value)
    {
        if (Runner == null) return;
        if (IsMaster())
        {
            if (!Runner.IsServer)
            {
                RPC_Configure(value);
                BorderCrossCheck(float.Parse(value.Split(',')[1]));
            }
            else
            {
                Message = String.Copy(value);
            }
        }
        if (Runner.IsServer)
        {
            string[] values = value.Split(',');
            float elbowValue = float.Parse(values[1]);

            elbow.SetElbow(elbowValue, float.Parse(values[2]));
            BorderCrossCheck(elbowValue);
        }
    }

    private void BorderCrossCheck(float value)
    {
        if (AlertEnabled || BordersEnabled)
        {
            bool isValueBeyondBorder = value > UpperBorder || value < LowerBorder;
            if (!borderCrossed && isValueBeyondBorder)
            {
                Debug.Log("Crossed border " + value);
                OnBorderCrossed?.Invoke(value);
                borderCrossed = true;
            }
            if (borderCrossed && !isValueBeyondBorder)
            {
                Debug.Log("Within Border " + value);
                OnBorderCrossed?.Invoke(value);
                borderCrossed = false;
            }
        }
    }

    //Use RPC calls to tell the Server to update the message from a client.
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Configure(string message)
    {
        this.Message = message;
    }

    public void ChangeHost()
    {
        PlayerRef playerRef;
        if (HostUnchanged)
        {
            playerRef = Runner.ActivePlayers.First(s => s != Runner.LocalPlayer);
        }
        else
        {
            playerRef = Runner.LocalPlayer;
        }

        network.AssignInputAuthority(playerRef);
        this.HostUnchanged = !HostUnchanged;
    }

    public static void SendHost(Changed<PlayerNetworkController> changed)
    {
        changed.Behaviour.SendHost();
    }

    public void SendHost()
    {
        udpHost.SendMsg("master " + IsMaster());
    }

    public static void SendUpper(Changed<PlayerNetworkController> changed)
    {
        changed.Behaviour.SendUpper();
    }

    public void SendUpper()
    {
        udpHost.SendMsg("border upper " + UpperBorder);
    }

    public static void SendLower(Changed<PlayerNetworkController> changed)
    {
        changed.Behaviour.SendLower();
    }

    public void SendLower()
    {
        udpHost.SendMsg("border lower " + LowerBorder);
    }

    public static void EnableBorders(Changed<PlayerNetworkController> changed)
    {
        changed.Behaviour.EnableBorders();
    }

    public void EnableBorders()
    {
        udpHost.SendMsg("border " + BordersEnabled);
    }

    public static void SendMirrorMessage(Changed<PlayerNetworkController> changed)
    {
        changed.Behaviour.SendMirrorMessage();
    }

    public void SendMirrorMessage()
    {
        udpHost.SendMsg(MirrorMessage);
    }

    public static void SendReleaseMessage(Changed<PlayerNetworkController> changed)
    {
        changed.Behaviour.SendReleaseMessage();
    }

    public void SendReleaseMessage()
    {
        udpHost.SendMsg("repeat " + !ReleaseControl);
    }

    public static void StartedBorders(Changed<PlayerNetworkController> changed)
    {
        changed.Behaviour.StartedBorders();
    }

    public void StartedBorders()
    {
        udpHost.SendMsg("log border");
    }

    private bool IsMaster()
    {
        if (network == null)
        {
            network = GetComponent<NetworkObject>();
        }
        return network.HasInputAuthority;
    }

    public bool HasHostChanged()
    {
        return HostUnchanged;
    }

    public float SetUpperBorder()
    {
        string[] values = Message.Split(',');
        this.UpperBorder = float.Parse(values[1]);
        return this.UpperBorder;
    }

    public float SetLowerBorder()
    {
        string[] values = Message.Split(',');
        this.LowerBorder = float.Parse(values[1]);
        return this.LowerBorder;
    }

    public void SetBorders()
    {
        BordersEnabled = !BordersEnabled;
    }

    public void SetAlert()
    {
        AlertEnabled = !AlertEnabled;
    }

    public void SetControl()
    {
        ReleaseControl = !ReleaseControl;
    }
}