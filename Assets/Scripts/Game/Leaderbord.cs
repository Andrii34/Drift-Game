using TMPro;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using System;
using System.Collections;
using Photon.Pun.UtilityScripts;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject _playersHolder; 
    
    [SerializeField] private GameObject[] _slots;
    [SerializeField] private TMP_Text[] _nameTexts;
    [SerializeField] private TMP_Text[] _scoreTexts;
    private void OnEnable()
    {
        DriftManager.OnDriftEnded += ShowLeaderBord;
    }
    private void OnDisable()
    {
        DriftManager.OnDriftEnded -= ShowLeaderBord;
    }

    private IEnumerator RefreshRate()
    {
        
            
            _playersHolder.SetActive(true);
            foreach (var slot in _slots)
            {
                slot.SetActive(false);
            }
            var sortedPlayersList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player ).ToList();
            int i = 0;
            foreach(var player in sortedPlayersList)
            {
                _slots[i].SetActive(true);
                if (player.NickName == "")
                {
                    player.NickName = "unnamed";
                }
                _nameTexts[i].text = player.NickName;
                _scoreTexts[i].text = player.GetScore().ToString();

                i++;
            }
        yield return new WaitForSeconds(1f);
            _playersHolder.SetActive(false);
        


    }
    public void ShowLeaderBord()
    {
        StartCoroutine(RefreshRate());
    }

}
