using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using YVR.Platform;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.SqlLobby);

    public AccountData currentLoginUser;

    public GameObject jointOrCreateRoomUI;

    private bool inRoom;
    private bool reconnectCalled;
    private DisconnectCause previousDisconnectCause;
    private bool rejoinCalled;
    private RoomOptions m_RoomOptions = new RoomOptions();

    private void Start()
    {
#if !UNITY_EDITOR
        YVRPlatform.Initialize();
#endif

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        GetLoginUserInfo();
        m_RoomOptions.PublishUserId = true;
    }

    public void GetLoginUserInfo()
    {
#if UNITY_EDITOR
        Debug.Log("SetPhotonConnect");
        SetPhotonConnect("8888888","TestUser01");
#else
        YVRRequest<AccountData> request = Account.GetLoggedInUser();
        request.OnComplete(OnGetLoginUser);
#endif

    }

    private void OnGetLoginUser(YVRMessage<AccountData> accountData)
    {
        currentLoginUser = accountData.data;
        LogController.Instance.Log($"Set local player nick name:{currentLoginUser.nickname}, userId:{currentLoginUser.accountID}");
        SetPhotonConnect(currentLoginUser.accountID.ToString(),currentLoginUser.nickname);
    }

    private void SetPhotonConnect(string userId,string userName)
    {
        AuthenticationValues authenticationValues = new AuthenticationValues();
        authenticationValues.UserId = userId;
        PhotonNetwork.AuthValues = authenticationValues;
        PhotonNetwork.LocalPlayer.NickName = userName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "cn";
        PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime =
            "20e53d27-067b-4ee2-8f21-67bdc01100b1"; // TODO: replace with your own AppId
        PhotonNetwork.PhotonServerSettings.AppSettings.Server = "ns.photonengine.cn";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LogController.Instance.Log(
            $"OnDisconnected(cause={cause}) ClientState={PhotonNetwork.NetworkingClient.State} PeerState={PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState}",
            true);
        if (this.rejoinCalled)
        {
            LogController.Instance.Log(
                $"Rejoin failed, client disconnected, causes; prev.:{this.previousDisconnectCause} current:{cause}",
                true);
            this.rejoinCalled = false;
        }
        else if (this.reconnectCalled)
        {
            LogController.Instance.Log(
                $"Reconnect failed, client disconnected, causes; prev.:{this.previousDisconnectCause} current:{cause}",
                true);
            this.reconnectCalled = false;
        }

        this.HandleDisconnect(cause); // add attempts counter? to avoid infinite retries?
        this.inRoom = false;
        this.previousDisconnectCause = cause;
    }

    private void HandleDisconnect(DisconnectCause cause)
    {
        switch (cause)
        {
            // cases that we can recover from
            case DisconnectCause.ServerTimeout:
            case DisconnectCause.Exception:
            case DisconnectCause.ClientTimeout:
            case DisconnectCause.DisconnectByServerLogic:
            case DisconnectCause.AuthenticationTicketExpired:
            case DisconnectCause.DisconnectByServerReasonUnknown:
                if (this.inRoom)
                {
                    LogController.Instance.Log("calling PhotonNetwork.RejoinRoom()");
                    this.rejoinCalled = PhotonNetwork.RejoinRoom("ShareAnchorRoom");
                    if (!this.rejoinCalled)
                    {
                        LogController.Instance.Log(
                            "PhotonNetwork.RejoinRoom returned false, PhotonNetwork.Reconnect is called instead.",
                            true);
                        this.reconnectCalled = PhotonNetwork.Reconnect();
                    }
                }
                else
                {
                    LogController.Instance.Log("calling PhotonNetwork.Reconnect()");
                    this.reconnectCalled = PhotonNetwork.Reconnect();
                }

                if (!this.rejoinCalled && !this.reconnectCalled)
                {
                    LogController.Instance.Log(
                        "PhotonNetwork.ReconnectAndRejoin() or PhotonNetwork.Reconnect() returned false, client stays disconnected.",
                        true);
                }

                break;
            case DisconnectCause.None:
            case DisconnectCause.OperationNotAllowedInCurrentState:
            case DisconnectCause.CustomAuthenticationFailed:
            case DisconnectCause.DisconnectByClientLogic:
            case DisconnectCause.InvalidAuthentication:
            case DisconnectCause.ExceptionOnConnect:
            case DisconnectCause.MaxCcuReached:
            case DisconnectCause.InvalidRegion:
                LogController.Instance.Log(
                    $"Disconnection we cannot automatically recover from, cause: {cause}, report it if you think auto recovery is still possible",
                    true);
                break;
        }
    }


    public void JointOrCreateRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, "ShareAnchorRoom",m_RoomOptions);
    }

    public override void OnConnected()
    {
        LogController.Instance.Log($"Connected to internet");
    }

    public override void OnLeftRoom()
    {
        LogController.Instance.Log("Left room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        LogController.Instance.Log("Failed to create room returnCode:" + returnCode + " message:" + message, true);
    }

    public override void OnConnectedToMaster()
    {
        LogController.Instance.Log("Connected to master");
        PhotonNetwork.JoinLobby(customLobby);
        if (this.reconnectCalled)
        {
            LogController.Instance.Log("Reconnect successful");
            this.reconnectCalled = false;
            PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, "ShareAnchorRoom",m_RoomOptions);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (this.rejoinCalled)
        {
            LogController.Instance.Log($"Quick rejoin failed with error code: {returnCode} & error message: {message}");
            this.rejoinCalled = false;
            PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, "ShareAnchorRoom",m_RoomOptions);
        }
    }

    public override void OnCreatedRoom()
    {
        LogController.Instance.Log("Created room success");
        jointOrCreateRoomUI.SetActive(false);
        inRoom = true;
    }

    public override void OnJoinedLobby()
    {
        LogController.Instance.Log("Joined lobby success");
        Debug.Log(PhotonNetwork.LocalPlayer.UserId);
    }

    public override void OnLeftLobby()
    {
        LogController.Instance.Log("Left lobby");
    }

    public override void OnJoinedRoom()
    {
        LogController.Instance.Log("Joined room success");
        LogController.Instance.Log($"Joint room name:{PhotonNetwork.CurrentRoom.Name}");
        jointOrCreateRoomUI.SetActive(false);
        inRoom = true;
        if (this.rejoinCalled)
        {
            LogController.Instance.Log("Rejoin successful");
            this.rejoinCalled = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // LogController.Instance.Log("Player entered room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        LogController.Instance.Log("Player left room");
        this.inRoom = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LogController.Instance.Log($"Failed to join random room return code:{returnCode},message:{message}");
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        LogController.Instance.Log($"Error info:{errorInfo.Info}");
    }
}