﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject Loading;

    public GameObject JoinButton;

    public GameObject CreateButton;

    public GameObject QuitButton;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    public override void OnConnectedToMaster()
    {
        Loading.SetActive(false);

        JoinButton.SetActive(true);

        CreateButton.SetActive(true);

        QuitButton.SetActive(true);

        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom()
    {
        StartGame();

        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();

        base.OnJoinRandomFailed(returnCode, message);
    }

    public void Connect()
    {
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create()
    {
        PhotonNetwork.CreateRoom("");
    }

    public void StartGame()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
