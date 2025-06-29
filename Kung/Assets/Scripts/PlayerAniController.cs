using UnityEngine;

public class PlayerAniController : MonoBehaviour
{
    //왼쪽 - 채굴중ㅇㅣㄹ결ㅇㅜ, 아니ㄹ겨ㅇㅇㅜ
    //오른쪽 - 채구ㄹㅈㅜㅇ일겨ㅇㅇㅜ,아니ㄹ겨ㅇㅇㅜ
    //idle - 채구ㄹㅈㅜㅇ일겨ㅇㅇㅜ
    [Header("애니메이터")]
    [SerializeField] private Animator _headAnimator;
    [SerializeField] private Animator _bodyAnimator;
    [SerializeField] private Animator _boostAnimator;
    [SerializeField] private Animator _drillLeft;
    [SerializeField] private Animator _drillRight;

    [Header("스프라이트")]
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private SpriteRenderer _headSprite;
    [SerializeField] private Sprite[] _drillingSprite;

    public void OnLeftAniPlay(bool isDrilling)
    {
        if (isDrilling)
        {
        }
        else
        {

        }
    }
    
}
