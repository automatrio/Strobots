using Godot;
using FlatBuffers;
using System.Collections.Generic;
using NetworkPacket;
using Steamworks;
using System.Collections.ObjectModel;
using Multiplayer;
using System;

public class NetworkDataExchange : Node
{
    // exports
    [Export] public NodePath FamilyPath;
    [Export] public float DataExchangeInterval = 0.1f;

    // fields
    private Timer _timer = new Timer();
    private CSteamID _targetPlayer; // expand to accomodate more players

    // properties
    public ObservableCollection<string> DataExchangeExceptions { get; set; } = new ObservableCollection<string>();
    public SurrogateData[] SurrogateData = new SurrogateData[4];

    // children
    private Family family;
    private Player player;
    private List<Robot> robots = new List<Robot>();

    // steam callbacks
    protected Callback<P2PSessionRequest_t> Callback_P2PSessionRequest;
    protected Callback<P2PSessionConnectFail_t> Callback_P2PSessionConnectFail;

    // signaling
    public event EventHandler SurrogateDataReceived;

    public override void _Ready()
    {
        family = GetNode<Family>(FamilyPath);

        populateDependencies();

        configureTimer(DataExchangeInterval);

        getTargetPlayer(out _targetPlayer);

        configureSteamCallbacks();

    }

    public async override void _PhysicsProcess(float delta)
    {
        await ToSignal(_timer, "timeout");

        sendPlayerData(_targetPlayer, player);
        SurrogateData[0] = receiveData(0);

        sendPlayerData(_targetPlayer, robots[0]);
        SurrogateData[1] = receiveData(1);

        sendPlayerData(_targetPlayer, robots[1]);
        SurrogateData[2] = receiveData(2);

        sendPlayerData(_targetPlayer, robots[2]);
        SurrogateData[3] = receiveData(3);

        GetNode<Surrogates>("Surrogates").UpdateSurrogateData();
    }
    
    
    private void populateDependencies()
    {
        // get the player and the robots from the family
        foreach(Node child in family.GetChildren())
        {
            if(child is Player)
            {
                player = child as Player;
            }
            else if(child is Robot)
            {
                robots.Add(child as Robot);
            }
            else
            {
                return;
            }
        }
    }

    private void configureTimer(float waitTime)
    {
        _timer.ProcessMode = Timer.TimerProcessMode.Physics;
        _timer.WaitTime = waitTime;
        _timer.Autostart = true;
        GetTree().Root.AddChild(_timer);
    }

    private static void getTargetPlayer(out CSteamID playerToReceiveData)
    {
        if(MultiplayerGlobals.IsPlayingAsHost)
        {
            playerToReceiveData = MultiplayerGlobals.Player1_ID;
        }
        else
        {
            playerToReceiveData = MultiplayerGlobals.Player2_ID;
        }
    }

    // send data logic
    private static byte[] createPlayerData(out uint length, in Spatial entity)
    {
        // Creating a new packet (byte array)
        FlatBufferBuilder builder = new FlatBufferBuilder(24);
        EntityData.StartEntityData(builder);

        // insert player position
        EntityData.AddPos(
            builder,
            Vec3.CreateVec3
            (
                builder, 
                entity.GlobalTransform.origin.x,
                entity.GlobalTransform.origin.y,
                entity.GlobalTransform.origin.z
            ));

        // expand to be able to transfer state machine data
        EntityData.AddState(
            builder,
            0
        );

        // stop building the packet
        var stopBuilding = EntityData.EndEntityData(builder);
        builder.Finish(stopBuilding.Value);

        var packet = builder.SizedByteArray();

        length = (uint)packet.Length;

        // convert it to a byte array
        return packet;
    }

    private static void sendPlayerData(CSteamID playerID, Spatial entity)
    {
        uint packetLength;

        SteamNetworking.SendP2PPacket(
            playerID,
            createPlayerData(out packetLength, entity),
            packetLength,
            EP2PSend.k_EP2PSendUnreliable
            );
    }

    // P2P session procedures
    private void configureSteamCallbacks()
    {
        Callback_P2PSessionRequest = Callback<P2PSessionRequest_t>.Create(onP2PSessionRequest);
        Callback_P2PSessionConnectFail = Callback<P2PSessionConnectFail_t>.Create(onP2PSessionConnectFail);
    }

    private void onP2PSessionRequest(P2PSessionRequest_t request)
    {
        if
        (
            request.m_steamIDRemote == MultiplayerGlobals.Player1_ID ||
            request.m_steamIDRemote == MultiplayerGlobals.Player2_ID
        )  
        {
            SteamNetworking.AcceptP2PSessionWithUser(request.m_steamIDRemote);
        }
        else
        {
            DataExchangeExceptions.Add("A connection was just rejected.");
        }
    }

    private void onP2PSessionConnectFail(P2PSessionConnectFail_t fail)
    {
        DataExchangeExceptions.Add($"ERROR:{fail.m_eP2PSessionError}. \n P2P session failed.");
    }

    // receive data logic
    private static SurrogateData receiveData(in int dataIndex)
    {

        SurrogateData surrogateData = new SurrogateData();

        if(SteamNetworking.IsP2PPacketAvailable(out uint packetSize))
        {
            byte[] incomingPacket = new byte[packetSize];

            if(SteamNetworking.ReadP2PPacket(incomingPacket, packetSize, out uint bytesRead, out CSteamID remoteID))
            {
                // Flatbuffers: create a new buffer from the incoming byte[]
                ByteBuffer buffer = new ByteBuffer(incomingPacket);
                
                // populate the surrogate data array with all the information
                surrogateData = new SurrogateData
                    (
                        ( (Vec3) EntityData.GetRootAsEntityData(buffer).Pos).ToVector3()
                    );
            }
        }

        return surrogateData;
    }
}