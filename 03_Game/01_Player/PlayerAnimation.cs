using UnityEngine;

public class PlayerAnimation
{
    public const string RunParam = "Run";
    public const string DieParam = "Die";

    private Animator _animator;

    public PlayerAnimation(Animator animator)
    {
        _animator = animator;
    }

    public void Idle()
    {
        _animator.SetBool(RunParam, false);
    }

    public void Run()
    {
        _animator.SetBool(RunParam, true);
    }

    public void Die()
    {
        _animator.SetBool(DieParam, true);
    }

    public void Revive()
    {
        _animator.SetBool(DieParam, false);
    }
}
