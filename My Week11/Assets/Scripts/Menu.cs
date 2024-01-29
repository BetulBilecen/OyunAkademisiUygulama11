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
        PhotonNetwork.NickName = playerNameInput.text; //Oyuncu isminin eþitliyoruz burada
    }

    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);
        //KULLANICI HER EKLEME/SÝLME ÝÞLEMÝNDE UpdateLobbyUI() FONKÝYONUNU ÇAÐIRMAK VE DÝÐER KULLANICILARA NE ÝÞLEM YAILDIÐINI GÖSTERMEK ÝÇÝN:
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    //YENÝ EKLENEN KATILIMCILAI UPDATE ETMEKÝÇÝN:
    [PunRPC]//Güncellenen lobby'yi herkese duyurmak için
    public void UpdateLobbyUI()
    {
        playerListText.text = ""; //Mevcut liseyi temizledi

        foreach (Player player in PhotonNetwork.PlayerList) //Bütün katýlýmcý listesi için PhotonNetwork.PlayerList burada dolaþmak içinde foreach döngüsünü yazdýk
        {
            playerListText.text += player.NickName + "\n"; // Yeni katýlýmcý listesi oluþturduk.
        }

        if (PhotonNetwork.IsMasterClient)
            startGameButton.interactable = true;
        else
            startGameButton.interactable = false;
    }

    //AYRILMAYI HERKESE DUYURDUKTAN SONRA KATILIMCI LÝSTESÝNÝ DÜZENLEMEK ÝÇÝN:
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    //OYUNDAN AYRILMAK ÝÇÝN:
    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen); //ana ekrana dönüþ saðlandý olur da baþka bir odaya girip o odada oynamak isterse diye
    }

    public void OnStartGameButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
}
