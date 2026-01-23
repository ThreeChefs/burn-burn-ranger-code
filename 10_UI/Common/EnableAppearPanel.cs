
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnableAppearPanel : MonoBehaviour
{
    Vector3 originPos;
    CanvasGroup cg;

    [SerializeField] Transform contetns;

    Vector3 contentsPos;

    void Awake()
    {
        originPos = transform.position;

        cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();

        if (contetns != null)
            contentsPos = contetns.localPosition;
    }

    void OnEnable()
    {
        cg.DOKill();
        contetns?.DOKill();


        cg.alpha = 0f;

        if (contetns != null)
            contetns.localPosition = contentsPos + Vector3.down * 50f;

        cg.DOFade(1f, 0.5f).SetUpdate(true).SetDelay(0.5f);

        if (contetns != null)
            contetns.DOLocalMove(contentsPos, 0.5f).SetEase(Ease.OutCubic).SetUpdate(true).SetDelay(0.5f);
    }

}
