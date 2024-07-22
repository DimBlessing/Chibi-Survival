using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    //업적, 해금 관리
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    void Init(){
        PlayerPrefs.SetInt("MyData", 1);
    }

    void Start()
    {
        
    }
    void UnlockCharacter(){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
