using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDatos : MonoBehaviour
{
    [System.Serializable]
    public struct AttackData
    {
        public string name;
        public int damage;
    }
}
