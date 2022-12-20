using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    private float applyRunSpeed;
    private bool applyRunFlag = false;

    private Vector3 vetcor; //캐릭터의 x,y,z 좌표값

    public int walkCount;
    private int currentWalkCount;

    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator MoveCoroutine()
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
