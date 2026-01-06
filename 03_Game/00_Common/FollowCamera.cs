using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform _target;
    private Vector3 _offset;

    private bool _lockX;
    private bool _lockY;

    /// <summary>
    /// [public] 스테이지 시작 시 플레이어와 카메라 연결
    /// </summary>
    /// <param name="lockX">x 좌표 이동 잠금</param>
    /// <param name="lockY">y 좌표 이동 잠금</param>
    public void ConnectPlayer(bool lockX = false, bool lockY = false)
    {
        if (PlayerManager.Instance.StagePlayer != null)
        {
            _target = PlayerManager.Instance.StagePlayer.transform;
            _offset = transform.position - _target.position;
        }

        _lockX = lockX;
        _lockY = lockY;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            Vector3 targetPos = transform.position;
            targetPos.x += _lockX ? 0 : (_target.position.x - transform.position.x) + _offset.x;
            targetPos.y += _lockY ? 0 : (_target.position.y - transform.position.y) + _offset.y;
            transform.position = targetPos;
        }
    }
}
