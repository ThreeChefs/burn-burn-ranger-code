using UnityEngine;

/// <summary>
/// 맵 재배치
/// </summary>
public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.PlayerTag))
        {
            Vector3 diff = PlayerManager.Instance.StagePlayer.transform.position - transform.position;
            Vector3 movePos = transform.position;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                movePos += (diff.x > 0 ? 1 : -1) * Define.MapSize * 2 * Vector3.right;
            }
            else
            {
                movePos += (diff.y > 0 ? 1 : -1) * Define.MapSize * 2 * Vector3.up;
            }

            transform.position = movePos;
        }
    }
}
