using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    private LaserPattern owner;
    public void Bind(LaserPattern Laser) => owner = Laser;

    private void OnTriggerStay2D(Collider2D other)
    {
        owner?.ApplyTickDamage(other);
    }
}
