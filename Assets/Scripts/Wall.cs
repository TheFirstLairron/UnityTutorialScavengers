using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public Sprite damageSprite;

    public int hp = 4;

    private SpriteRenderer spriteRenderer;

	private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = damageSprite;
        hp -= loss;

        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
