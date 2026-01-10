using UnityEngine;

public class GemItem : MonoBehaviour
{
    [SerializeField] private GemItemData itemData;
    [SerializeField] private float magnetSpeed = 18f;

    private Transform _magnetTarget; //자석의 타겟 플레이어로 지정
    private bool _isMagneting;

    public void StartMagnet(Transform target)
    {
        _magnetTarget = target;  // 따라갈 대상 
        _isMagneting = true;
    }

    public int ExpValue => itemData != null ? itemData.expValue : 0;

    private void Update()
    {
        if (!_isMagneting || _magnetTarget == null)  //자석을 먹지않으면 아무것도 안함.
            return;

        transform.position = Vector3.MoveTowards(transform.position, _magnetTarget.position, magnetSpeed * Time.deltaTime);
        // 현재위치 - 플레이어 위치로 magnetSpeed만큼 속도로 빨려들어감.



    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<StagePlayer>(out var player))
            return;
        player.StageLevel.AddExp(itemData.expValue);
        Destroy(gameObject);
    }
}
