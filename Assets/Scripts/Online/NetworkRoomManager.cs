using Photon.Pun;

using System.Collections.Generic;


using UnityEngine;

public class NetworkRoomManager : MonoBehaviourPunCallbacks
{
    public static NetworkRoomManager instance;
    [SerializeField] private GameObject _player;
  

    [Space]
    [SerializeField] private GameObject _rumCamera;

    [Space]
    [SerializeField] private Transform[] _spawnPoints;

    [Space]
    [SerializeField] private GameObject _nameUI;
    [SerializeField] private GameObject _connectingUI;


    private string _nickname = "player";
    public void ChangeNickName(string name)
    {
        _nickname = name;
        

    }
    public void JoinRoomButtonPressed()
    {

        Debug.Log("Conecting...");
        PhotonNetwork.ConnectUsingSettings();
        _nameUI.SetActive(false);
         _connectingUI.SetActive(true);

    }
    private void Awake()
    {
        _nameUI.SetActive(true);
        instance = this;
    }
    
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Conect to server");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("test", null, null);

    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("мы создали и находимся в комнате");
        _rumCamera.SetActive(false);
        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        Transform spawnPoint = GetNextSpawnPoint();
        if (spawnPoint == null)
        {
            Debug.LogError("Нет доступных точек спауна!");
            return;
        }

        // Создание игрока
        GameObject player = PhotonNetwork.Instantiate(_player.name, spawnPoint.position, Quaternion.identity);

        // Получаем PhotonView объекта игрока
        PhotonView playerPhotonView = player.GetComponent<PhotonView>();

        if (playerPhotonView.IsMine)
        {
            player.GetComponent<PlayerSetings>().isLocalPlayer();

            // Отправляем RPC для задания имени игрока всем
            playerPhotonView.RPC("SetNickname", RpcTarget.AllBuffered, _nickname);
            PhotonNetwork.LocalPlayer.NickName = _nickname;
        }
    }
    private Transform GetNextSpawnPoint()
    {
        // Получить список занятых точек из Custom Properties комнаты
        HashSet<int> usedIndexes = GetUsedSpawnIndexes();

        // Найти первый свободный индекс
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (!usedIndexes.Contains(i))
            {
                // Пометить точку как занятую
                MarkSpawnPointAsUsed(i);
                return _spawnPoints[i];
            }
        }

        return null; // Все точки заняты
    }
    private HashSet<int> GetUsedSpawnIndexes()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("UsedSpawnPoints", out object value) && value is int[] indexes)
        {
            return new HashSet<int>(indexes);
        }
        return new HashSet<int>();
    }
    private void MarkSpawnPointAsUsed(int index)
    {
        // Получить текущий список занятых точек
        HashSet<int> usedIndexes = GetUsedSpawnIndexes();

        // Добавить новую точку
        usedIndexes.Add(index);

        // Сохранить обновленный список в Custom Properties комнаты
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "UsedSpawnPoints", new List<int>(usedIndexes).ToArray() }
        });
    }
}
