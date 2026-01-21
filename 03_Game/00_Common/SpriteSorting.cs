using UnityEngine;

// todo : dirtyflag 참고해보기
public class SpriteSorting : MonoBehaviour
{
    SpriteRenderer[] sprites;

    [SerializeField] SpriteSortingLayer _sprSortingLayer;

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>(true);


        if (_sprSortingLayer == SpriteSortingLayer.Unspecified) return; // 따로 설정하지 않기 (기존 프리팹에 설정되어있는대로 쓰기

        string layerName = GetLayerName(_sprSortingLayer);
        int layerId = SortingLayer.NameToID(layerName);     // 레이어 ID는 0,1,2.. 가 아니어서 레이어 이름을 가져와서 아이디를 넣어줘야함!

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingLayerID = layerId;
        }


    }
    
    private void LateUpdate()
    {
        for (int i = 0; i < sprites.Length; ++i)
        {
            sprites[i].sortingOrder = (int)(this.transform.position.y * 1000) * -1;
        }
    }



    private static string GetLayerName(SpriteSortingLayer layer)
    {
        return layer switch
        {
            SpriteSortingLayer.Default => "Default",
            SpriteSortingLayer.BackGround => "BackGround",
            SpriteSortingLayer.BackGroundDecal => "BackGroundDecal",
            SpriteSortingLayer.Item => "Item",
            SpriteSortingLayer.Character => "Character",
            SpriteSortingLayer.Projectile => "Projectile",
            SpriteSortingLayer.Effect => "Effect",
            SpriteSortingLayer.WorldUI => "WorldUI",
            _ => "Default",     // 이넘 와일드카드
        };
    }

}


public enum SpriteSortingLayer
{
    Unspecified,
    Default,
    BackGround,
    BackGroundDecal,
    Item,
    Character,
    Projectile,
    Effect,
    WorldUI,
}

