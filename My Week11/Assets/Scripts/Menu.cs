using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject lobbyScreen;

    [Header("Main Screen")]
    public Button createRoomButton;
    public Button joinRoomButton;

    [Header("Lobby Screen")]
    public TextMeshProUGUI playerListText;
    public Button startGameButton;

    private void Start()
    {
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
    }
    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }

    void SetScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        screen.SetActive(true);
    }

    public void OnCreateRoomButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void OnJoinRoomButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }

    public void OnPlayerNameUpdate(TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text; //Oyuncu isminin e�itliyoruz burada
    }

    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);
        //KULLANICI HER EKLEME/S�LME ��LEM�NDE UpdateLobbyUI() FONK�YONUNU �A�IRMAK VE D��ER KULLANICILARA NE ��LEM YAILDI�INI G�STERMEK ���N:
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    //YEN� EKLENEN KATILIMCILAI UPDATE ETMEK���N:
    [PunRPC]//G�ncellenen lobby'yi herkese duyurmak i�in
    public void UpdateLobbyUI()
    {
        playerListText.text = ""; //Mevcut liseyi temizledi

        foreach (Player player in PhotonNetwork.PlayerList) //B�t�n kat�l�mc� listesi i�in PhotonNetwork.PlayerList burada dola�mak i�inde foreach d�ng�s�n� yazd�k
        {
            playerListText.text += player.NickName + "\n"; // Yeni kat�l�mc� listesi olu�turduk.
        }

        if (PhotonNetwork.IsMasterClient)
            startGameButton.interactable = true;
        else
            startGameButton.interactable = false;
    }

    //AYRILMAYI HERKESE DUYURDUKTAN SONRA KATILIMCI L�STES�N� D�ZENLEMEK ���N:
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    //OYUNDAN AYRILMAK ���N:
    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen); //ana ekrana d�n�� sa�land� olur da ba�ka bir odaya girip o odada oynamak isterse diye
    }

    public void OnStartGameButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
}
