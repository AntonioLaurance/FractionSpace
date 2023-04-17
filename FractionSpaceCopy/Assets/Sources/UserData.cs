using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    // Datos del jugador
    public Player player;
    public Verify verify;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // For avoid lose of data when you change the scene
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
