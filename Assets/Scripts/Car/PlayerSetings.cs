using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSetings : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    public string Nickname { get; set; }
    private ICarController _carController ;
    public UnityEvent<string> OnChangedName = new UnityEvent<string>();
    public void isLocalPlayer()
    {
        _carController = GetComponent<ICarController>();
        if ( _carController is MonoBehaviour carControler)
        {
            carControler.enabled = true;
        }
        _camera.SetActive(true);
    }
    [PunRPC]
    public void SetNickname(string name)
    {

        Nickname = name;
        OnChangedName?.Invoke(Nickname);
        
    }

}
