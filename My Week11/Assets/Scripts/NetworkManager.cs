using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


// KULALNICI ODALARI
public class NetworkManager : MonoBehaviourPunCallbacks //Hem monobehavior hemde pun �zelliklerini bar�nd�r�r
{
    //HERB�R OYUNCU KATILIMI ���N OTURUM A�MA:
    public static NetworkManager instance;
    private void Awake() //Sahneler olu�madan �nce yap�lan �elerdir. Biz oturum i�lemlerini yapt�k bu oyunda
    {
        if (instance != null && instance != this) //Yeni a��lan oturum �ncekinden farkl�ysa
        {
            gameObject.SetActive(false); //A��lan bir oturum varsa yeni oturum a�ma a��lanla devam et
        }
        else
        {
            instance = this; //Kullan�c�n�n a�t��� oturum bu anlam�nda
            DontDestroyOnLoad(gameObject); //Oyunu kapatma
        }
    }
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//Photnla ba�lat� kuruldu

    }

    ////KEND� FONKS�YON �SM�YLE O FONKS�YONDAK� DEY�MLER �ALI�TIYSA  BUNU B�LD�R�R
    //// BU ��LEM� GENELDE  override ANAHTAR KEL�MES�YLE YAPAR override SONRA B�LD�R�M VERL�ECEK FONKS�YON�SM�
    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Master server ile ba�lant� sa�land�.");
    //    CreateRoom("OyunSalonu 1"); //Create() fonksiyonunu �a��r�p "OyunSalonu 1" adl� bir oda olu�turuldu
    //}


    //ODA OLU�TURMA:
    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    //public override void OnCreatedRoom()
    //{
    //    Debug.Log("Belirtilen isimde oda olu�turuldu. Oda ismi: " + PhotonNetwork.CurrentRoom.Name); // Olu�turulan odan�n ismi PhotonNetwork.CurrentRoom.Name
    //}

    //OLU�TURULAN ODAYA KATILMA:
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    //EKRAN DE���T�RME:
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

}
