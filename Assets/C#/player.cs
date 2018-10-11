using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour {

    public Transform Camera;
    public Text Grade;
    private Rigidbody rigidbodyPlayer;  //刚体属性
    public float F;     //作用力大小
    private float downTime;     //按下时间
    public GameObject Cube;     //创建的cube模板
    private GameObject CurrentCube;     //当前cube    
    private GameObject NextCube;        //下一个cube
    private Collider Lastcollision;
    private int grade;
    private Vector3 CameraRelativePosition;     //相机相对位置
    private Vector3 F_WAY;
    // Use this for initialization
    void Start () {
        rigidbodyPlayer = GetComponent<Rigidbody>();        //获取刚体组件
        CurrentCube = Cube;
        Lastcollision = CurrentCube.GetComponent<Collider>();
        ProduceCube(Cube);
        grade = 0;
        Grade.text =""+grade.ToString();
        CameraRelativePosition = Camera.position - transform.position;  //相机位置-小人位置
        
    }
	
	// Update is called once per frame
	void Update () {
        F_WAY = NextCube.transform.position-transform.position ;
        F_WAY = F_WAY.normalized+Vector3.up*0.9f;
        Time.timeScale = 1.8f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            downTime = Time.time;
        }
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began: downTime = Time.time;break;
                case TouchPhase.Ended:
                    float upTime = Time.time-downTime;
                    Jump(upTime);
                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            float upTime = Time.time - downTime;
            Jump(upTime);
        }
    }

    void Jump(float time)
    {
        rigidbodyPlayer.AddForce(F_WAY * F * time,ForceMode.Impulse);        //设置力的类型为瞬间的力
    }

    void ProduceCube(GameObject gameObject) 
    {
         NextCube = Instantiate(gameObject);
        NextCube.transform.position = gameObject.transform.position + new Vector3(Random.Range(6f, 8f), 0, Random.Range(-6f, 8f));
;    }

    private void OnCollisionEnter(Collision collision)      //判断物体碰撞
    {
        Debug.Log(collision.gameObject.name);   //输出被碰撞物体名称
        if(collision.gameObject.name.Contains("Cube")&& collision.collider != Lastcollision)    //若碰撞到的不是当前物体（棋子脚下的物体）且物体名为cube
        {
            Destroys();
            Lastcollision = collision.collider;
            CurrentCube = collision.gameObject;
            ProduceCube(CurrentCube);
            MoveCamera();
            if ((transform.position.x - CurrentCube.transform.position.x) < 0.3f&& (transform.position.x - CurrentCube.transform.position.x) > -0.3f)   //中心加分
            {
                grade += 2;
                Grade.text = "" + grade.ToString();
            }
            else
            {
                grade ++;
                Grade.text = "" + grade.ToString();
            }
            Debug.Log(transform.position.x - CurrentCube.transform.position.x);
        }

        if (collision.gameObject.name == "Plane")
        {
            //重头开始加载场景
            SceneManager.LoadScene(0);
        }
    }

    private void MoveCamera()   //移动相机
    {

        Camera.DOMove(transform.position + CameraRelativePosition, 1);
    }

    private void Destroys()
    {
        Destroy(CurrentCube, 3);    //删除原来的cube
        CurrentCube = NextCube;
    }
}
