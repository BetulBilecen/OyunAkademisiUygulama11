using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


// KULALNICI ODALARI
public class NetworkManager : MonoBehaviourPunCallbacks //Hem monobehavior hemde pun özelliklerini barýndýrýr
{
    //HERBÝR OYUNCU KATILIMI ÝÇÝN OTURUM AÇMA:
    public static NetworkManager instance;
    private void Awake() //Sahneler oluþmadan önce yapýlan þelerdir. Biz oturum iþlemlerini yaptýk bu oyunda
    {
        if (instance != null && instance != this) //Yeni açýlan oturum öncekinden farklýysa
        {
            gameObject.SetActive(false); //Açýlan bir oturum varsa yeni oturum açma açýlanla devam et
        }
        else
        {
            instance = this; //Kullanýcýnýn açtýðý oturum bu anlamýnda
            DontDestroyOnLoad(gameObject); //Oyunu kapatma
        }
    }
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//Photnla baðlatý kuruldu

    }

    ////KENDÝ FONKSÝYON ÝSMÝYLE O FONKSÝYONDAKÝ DEYÝMLER ÇALIÞTIYSA  BUNU BÝLDÝRÝR
    //// BU ÝÞLEMÝ GENELDE  override ANAHTAR KELÝMESÝYLE YAPAR override SONRA BÝLDÝRÝM VERLÝECEK FONKSÝYONÝSMÝ
    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Master server ile baðlantý saðlandý.");
    //    CreateRoom("OyunSalonu 1"); //Create() fonksiyonunu çaðýrýp "OyunSalonu 1" adlý bir oda oluþturuldu
    //}


    //ODA OLUÞTURMA:
    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    //public override void OnCreatedRoom()
    //{
    //    Debug.Log("Belirtilen isimde oda oluþturuldu. Oda ismi: " + PhotonNetwork.CurrentRoom.Name); // Oluþturulan odanýn ismi PhotonNetwork.CurrentRoom.Name
    //}

    //OLUÞTURULAN ODAYA KATILMA:
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    //EKRAN DEÐÝÞTÝRME:
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

}
