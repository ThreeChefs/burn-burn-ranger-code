using UnityEngine;

public class SpriteSorting : MonoBehaviour
{
    SpriteRenderer[] sprites;

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }
    
    public void UpdateSorting(float pos)
    {
        for (int i = 0; i < sprites.Length; ++i)
        {
            sprites[i].sortingOrder = (int)(pos * 1000) * -1;
        }

    }
}
