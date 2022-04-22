using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ObjectLayerOrder : MonoBehaviour
{
    [SerializeField] int valueLayerOrder;
    Transform player;
    SkeletonAnimation playerSpriteRenderer;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameController.Instance.Player.transform;
        playerSpriteRenderer = player.GetChild(0).GetComponent<SkeletonAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y > player.transform.position.y + 0.7f)
        {
            spriteRenderer.sortingOrder = playerSpriteRenderer.GetComponent<MeshRenderer>().sortingOrder - (valueLayerOrder + 1);
        }
        else
        {
            spriteRenderer.sortingOrder = playerSpriteRenderer.GetComponent<MeshRenderer>().sortingOrder + (valueLayerOrder + 1);
        }
    }
}
