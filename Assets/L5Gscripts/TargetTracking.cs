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
    bool LeadOn = false;
    bool ShowFace = false;

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


    }

    private void DistChanger() // ユーザとエージェント間の距離によってdeistanceFarの値を変更
    {
        float dist = Vector3.Distance(agent.transform.position, master.transform.position);
        if (dist > 2.5)
        {
            distanceFar.Value = true;

        }
        //else
        if (dist < 2.0)
        {
            distanceFar.Value = false;
        }
    }

    public void AgentMoveWithMaster()　// desitanceFarの値によって、ユーザに近ければ進みユーザから離れれば止まるようになる
    {
        
        if(distanceFar.Value)
        {
            Debug.Log("true");
            StopDestination();
        }
        else
        {    
            SetDestination();
            Debug.Log("false");
        }
        
    }

    public void SetDestination() //targetの位置へ移動
    {
        if (LeadOn)
        {
            var endPoint = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            agent.destination = endPoint;
        }
        ShowFace = false;
    }

    public void StopDestination() //その場で留まる
    {
        if (LeadOn)
        {
            var stopPoint = new Vector3(slave.transform.position.x, slave.transform.position.y, slave.transform.position.z);
            agent.destination = stopPoint;
            ShowFace = true;
        }
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

    public void WalkFront() // ユーザの前に移動
    {
        WalkFree = false;
        agent.destination = master.transform.position + master.transform.forward * 1.0f;
        ShowFace = true;
    }
    
    
    private void OnAnimatorIK(int layorIndex) // 体と顔をユーザへ向ける処理
    {
        if (ShowFace)
        {
            animator.SetLookAtWeight(1.0f, 0.8f, 0.2f, 0.8f, 0.4f);
            animator.SetLookAtPosition(master.transform.position);
            /*var rotation = new Vector3(master.transform.position.x, agent.transform.position.y, master.transform.position.z);
            transform.rotation = Quaternion.LookRotation(rotation);
            ShowFace = false;*/
        }
    }
    

    public void GoFirst() //キーワード「ありがとう」から初期状態へ戻る処理
    {
        WalkFree = true;
        ShowFace = false;
        LeadOn = false;
    }

    public void ShowFaceToMaster() // ユーザへ体と顔を向ける処理に使われる変数ShowFaceの処理
    {
        ShowFace = true;
    }

    public void GuideStart() // 関数SetDestinationに使われるLeadOnの処理
    {
        LeadOn = true;
    }
}

    