using UnityEngine;

// todo : dirtyflag 참고해보기
public class SpriteSorting : MonoBehaviour
{
    SpriteRenderer[] sprites;

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }
    
    private void LateUpdate()
    {
        for (int i = 0; i < sprites.Length; ++i)
        {
            sprites[i].sortingOrder = (int)(this.transform.position.y * 1000) * -1;
        }
    }
}
