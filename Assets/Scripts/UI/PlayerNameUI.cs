using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameUI : MonoBehaviour
{
    private TMP_Text _name;
    void Start()
    {
        _name = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    public void ChangeNameText(string name)
    {
        if (_name == null)
        {
            _name = GetComponent<TMP_Text>();
        }
        _name.text = name;
        
        
    }

}
