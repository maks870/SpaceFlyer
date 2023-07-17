using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] float ofssetY;
    [SerializeField] Player player;

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + ofssetY, player.transform.position.z);
    }
}
