using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    // Datos del jugador
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream

=======
        
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream

=======
        
>>>>>>> Stashed changes
    }

    // For avoid lose of data when you change the scene
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
