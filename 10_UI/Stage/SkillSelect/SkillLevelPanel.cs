using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// todo 이거 일반 레벨 패널 따로 있어야 해
public class SkillLevelPanel : MonoBehaviour
{
    [SerializeField] private GameObject _normalGrade;
    [SerializeField] private GameObject _maxGrade;
    [SerializeField] private Image[] _normalGradeImgs;

    //private Coroutine nextGradeRoutine;

    public void Init(SkillType skillType, int currentLevel,bool showNextGrade = true)
    {
        if (skillType == SkillType.Combination)
        {
            _normalGrade.SetActive(false);
            _maxGrade.SetActive(true);
        }
        else
        {
            _normalGrade.SetActive(true);
            _maxGrade.SetActive(false);
            
            for (int i = 0; i < _normalGradeImgs.Length; i++)
            {
                if (currentLevel < i)
                {
                    _normalGradeImgs[i].gameObject.SetActive(false);
                }
                else if (currentLevel == i)
                {
                    if(showNextGrade)
                    {
                        _normalGradeImgs[i].gameObject.SetActive(true);

                        Color color = _normalGradeImgs[i].color;
                        color.a = 0.5f;
                        _normalGradeImgs[i].color = color;
                    }
                    else
                    {
                        _normalGradeImgs[i].gameObject.SetActive(false);
                    }
                }
                else
                {
                    _normalGradeImgs[i].gameObject.SetActive(true);
                    
                    Color color = _normalGradeImgs[i].color;
                    color.a = 1f;
                    _normalGradeImgs[i].color = color;
                }
            }
        }
        
      
    }

    // IEnumerator NextGradeRoutine()
    // {
    //     while (true)
    //     {
    //         yield return null;
    //     }
    // }
}