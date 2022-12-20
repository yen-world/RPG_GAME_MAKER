using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public LayerMask layerMask; // 통과 불가 레이어 설정 변수
    public float speed;
    public float runSpeed;
    private float applyRunSpeed;
    private bool applyRunFlag = false;

    private Vector3 vetcor; //캐릭터의 x,y,z 좌표값

    public int walkCount;
    private int currentWalkCount;

    private bool canMove = true;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    IEnumerator MoveCoroutine()
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            { // 왼쪽 shift 키를 눌렀을 때
                applyRunSpeed = runSpeed; // applyRunSpeed에 달리기 속도를 대입
                applyRunFlag = true;
            }
            else
            { // Shift를 누르지 않으면 달리는 중이 아님
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            vetcor.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if (vetcor.x != 0)
                vetcor.y = 0;

            animator.SetFloat("DirX", vetcor.x); // 애니메이션의 파라미터를 방향키에 따라 전달
            animator.SetFloat("DirY", vetcor.y);

            RaycastHit2D hit; // Ray를 쐈을 때 장애물이 맞으면 해당 장애물을 리턴해줌

            Vector2 start = transform.position; // 캐릭터의 현재 위치
            Vector2 end = start + new Vector2(vetcor.x * speed * walkCount, vetcor.y * speed * walkCount); // 캐릭터가 이동하고자 하는 위치

            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            if (hit.transform != null)
                break;

            animator.SetBool("Walking", true);


            while (currentWalkCount < walkCount)
            {
                if (vetcor.x != 0)
                { // 좌우 방향키가 눌렸을 경우
                    transform.Translate(vetcor.x * (speed + applyRunSpeed), 0, 0); // TransLate으로 위치값 변경
                }
                else if (vetcor.y != 0)
                { // 상하 방향키가 눌렸을 경우
                    transform.Translate(0, vetcor.y * (speed + applyRunSpeed), 0); // TransLate으로 위치값 변경
                }

                if (applyRunFlag)
                {
                    currentWalkCount++;
                }
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }

    }
}
