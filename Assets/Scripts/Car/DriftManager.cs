using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using UnityEngine.Events;

public class DriftManager : MonoBehaviour
{
    public Rigidbody playerRB;
    public TMP_Text totalScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text factorText;
    public TMP_Text driftAngleText;

    private float speed=0;
    private float driftAngle=0;
    private float driftFactor=1;
    private float currentScore;
    private float totalScore;

    private bool isDrifting = false;

    public float minimumSpeed = 5;
    public float minimumAngle = 10;
    public float driftingDelay = 0.2f;
    public GameObject driftingObject;
    public Color normalDriftColor;
    public Color nearStopColor;
    public Color driftEndedColor;
    public static event Action OnDriftEnded;
    private IEnumerator stopDriftingCoroutine = null;
    // Start is called before the first frame update
    void Start()
    {
        driftingObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ManageDrift();
        ManageUI();
    }
    void ManageDrift()
    {
        speed = playerRB.velocity.magnitude;
        driftAngle = Vector3.Angle(playerRB.transform.forward, (playerRB.velocity + playerRB.transform.forward).normalized);
        if (driftAngle > 120)
        {
            driftAngle = 0;
        }
        if (driftAngle >= minimumAngle && speed > minimumSpeed)
        {
            if(!isDrifting||stopDriftingCoroutine!=null)
            {
                StartDrift();
            }
        }
        else
        {
            if (isDrifting && stopDriftingCoroutine == null)
            {
                StopDrift();
            }
        }
        if (isDrifting)
        {
            currentScore += Time.deltaTime * driftAngle * driftFactor;
            driftFactor += Time.deltaTime;
            driftingObject.SetActive(true);
        }
    }

    async void StartDrift()
    {
        if (!isDrifting)
        {
            await Task.Delay(Mathf.RoundToInt(1000 * driftingDelay));
            driftFactor = 1;
        }
        if (stopDriftingCoroutine != null)
        {
            StopCoroutine(stopDriftingCoroutine);
            stopDriftingCoroutine = null;
        }
        currentScoreText.color = normalDriftColor;
        isDrifting = true;
    }
    void StopDrift()
    {
        stopDriftingCoroutine = StoppingDrift();
        StartCoroutine(stopDriftingCoroutine);
    }
    private IEnumerator StoppingDrift()
    {
        yield return new WaitForSeconds(0.1f);
        currentScoreText.color = nearStopColor;
        yield return new WaitForSeconds(driftingDelay * 4f);
        totalScore += currentScore;
        isDrifting = false;
        currentScoreText.color = driftEndedColor;
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.LocalPlayer.AddScore((int)totalScore);
        OnDriftEnded?.Invoke();
        currentScore = 0;
        driftingObject.SetActive(false);
    }

    void ManageUI()
    {
        totalScoreText.text = "Total: " + ((int)totalScore).ToString("###,###,000");
        factorText.text = driftFactor.ToString("###,###,##0.0")+"X";
        currentScoreText.text=currentScore.ToString("###,###,000");
        driftAngleText.text=driftAngle.ToString("###,##0") + "°";
    }
}








