using System.Data.Common;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum InputLockState
{
    None,
    Any,
    Left,
    Right
}
public class PlayerMovement : MonoBehaviour
{
    // 플레이어에 부착할 컴포넌트
    // 플레이어의 이동 제어

    [SerializeField] private float speed;
    private InputLockState currentState = InputLockState.Any;
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator drillAnimator;
    


   


    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && currentState != InputLockState.Left)
        {
            currentState = InputLockState.Right;
            transform.Translate(Vector3.left * speed * Time.deltaTime);
           
            ChangeAnimation("MoveLeft", bodyAnimator);
            ChangeAnimation("MoveLeft", headAnimator);
            ChangeAnimation("MoveLeft", drillAnimator);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && currentState != InputLockState.Right)
        {
            currentState = InputLockState.Left;
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            ChangeAnimation("MoveRight", bodyAnimator);
            ChangeAnimation("MoveRight", headAnimator);
            ChangeAnimation("MoveRight", drillAnimator);
        }
        else
        {
            currentState = InputLockState.Any;
           
            ChangeAnimation("Idle",bodyAnimator);
            ChangeAnimation("Idle",headAnimator);
        }


    }

    // 켜고싶은 불타입 애니메이션 파라미터 이름 , 애니메이터 넣어
    private void ChangeAnimation(string aniName, Animator animator)
    {
        var parameters = animator.parameters;

        foreach (var parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false);
            }
        }
        animator.SetBool(aniName, true);

    }
}
