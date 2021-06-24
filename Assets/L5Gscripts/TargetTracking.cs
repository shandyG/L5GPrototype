using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using UniRx;




public class TargetTracking : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public GameObject slave;
    public GameObject target;
    public GameObject master;
    ReactiveProperty<bool> distanceFar = new ReactiveProperty<bool>(false); //unirxによる値の監視
    bool WalkFree = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = transform.GetComponent<NavMeshAgent>();
        distanceFar.Subscribe(distanceFar => AgentMoveWithMaster());
        
        Observable.Timer(System.TimeSpan.Zero,System.TimeSpan.FromSeconds(15)).Subscribe(_ =>
        {
            WalkRandom();
        }).AddTo(this);
        
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude); //speedに合わせてモーションの速度を変更
        DistChanger(); //ユーザとエージェント間の距離を把握
        //RotateBodyToMaster();


    }

    public void AgentMoveWithMaster()　// desitanceFarの値によって、ユーザに近ければ進みユーザから離れれば止まるようになる
    {
        /*
        if(distanceFar.Value)
            Debug.Log("true");
        {
            StopDestination();
        }
        else
        {    
            SetDestination();
            Debug.Log("false");
        }
        */
    }

    private void DistChanger() // ユーザとエージェント間の距離によってdeistanceFarの値を変更
    {
        float dist = Vector3.Distance(agent.transform.position, master.transform.position);
        if (dist > 1.5)
        {
            distanceFar.Value = true;
        }
        else
        {
            distanceFar.Value = false;
        }
    }

    public void SetDestination() //targetの位置へ移動
    {
        var endPoint = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        agent.destination = endPoint;
    }

    public void StopDestination() //その場で留まる
    {
        var stopPoint = new Vector3(slave.transform.position.x, slave.transform.position.y, slave.transform.position.z);
        agent.destination = stopPoint;
    }

    public void WalkRandom() //masterの周りを1.5mの範囲内でランダムに移動
    {
        if (WalkFree)
        {
            float masterX = master.transform.position.x;
            float masterZ = master.transform.position.z;
            float agentX = Random.Range(masterX - 0.5f, masterX + 0.5f);
            float agentZ = Random.Range(masterZ - 0.5f, masterZ + 0.5f);
            var randomDestination = new Vector3(agentX, agent.transform.position.y, agentZ);
            agent.destination = randomDestination;
            Debug.Log("walk");
        }
    }

    public void WalkFront()
    {
        WalkFree = false;
        agent.destination = master.transform.position + master.transform.forward * 1.0f;
    }

    public void ShowFaceToMaster()
    {
        
    }

    private void OnAnimatorIK(int layorIndex)
    {
        //animator.SetLookAtWeight(1.0f, 0.8f, 1.0f, 0.0f, 0f);
        //animator.SetLookAtPosition(master.transform.position);
        //agent.destination = master.transform.position + master.transform.forward * 3.0f;
    }

    public void RotateBodyToMaster()
    {
        var rotation = new Vector3(master.transform.position.x, master.transform.position.y, master.transform.position.z);
        transform.rotation = Quaternion.LookRotation(rotation);
        Debug.Log("rotate");
    }

    public void GoFirst()
    {
        WalkFree = true;
    }
}

    