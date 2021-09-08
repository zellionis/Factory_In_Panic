using Com.IsartDigital.FactoryPanic.GameDesignProg.Narrative;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    // Start is called before the first frame update
    private int countGoodRob = 0;
    
    [SerializeField] private int NumRob = 0;

    [SerializeField] private float SpeedCarpet = 0.0f;

    [SerializeField]
    Transform StartCarpet = default;
    
    [SerializeField]
    Transform Quart1Carpet = default;
    
    [SerializeField]
    Transform Quart2Carpet = default;
    
    [SerializeField]
    Transform EndCarpet = default;
    
    [SerializeField]
    Transform Spawn = default;

    [SerializeField] 
    private float timeStopCarpet = 0.0f;
    
    private float CurrentTimeStop = 0.0f;
    
    private bool StopCarpet = false;
    
    private float TimeLerp = 0;
    
    List<GameObject> ListRobots = new List<GameObject>();
    
    [SerializeField] 
    List<GameObject> ListType = new List<GameObject>();
    
    private int[] choices = new int[3];

    [SerializeField]
    private NarrationManager narrationManager = default;
    
    void Start()
    {
        CreateRobot();
        for (int i = 0; i < 3; i++)
        {
            if (i < ListRobots.Count)
            {
                Robot rob = ListRobots[i].GetComponent<Robot>();
                if (rob)
                {
                    rob.Imatricule = i+1;
                }
            }
        }
        
        if(ListRobots.Count == 1)
            ListRobots[0].transform.position = Quart1Carpet.position;
        if(ListRobots.Count == 2)
            ListRobots[1].transform.position = Quart2Carpet.position;
        if(ListRobots.Count == 3)
            ListRobots[2].transform.position = EndCarpet.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!narrationManager.TextShowed)
        {
            if (StopCarpet)
                PauseCarpet();
            else
                MoveCarpet();
        }
    }

    void CreateRobot()
    {
        for (int i = 0; i < NumRob; i++)
        {
            GameObject go = Instantiate(ListType[0]);
            go.tag = "Robot";
            go.transform.position = Spawn.position;
            Robot rob = go.GetComponent<Robot>();
            rob.SetClassRobot(ClassRobot.Artiste, ClassRobot.Artiste);
            ListRobots.Add(go);
        }
    }

    void MoveCarpet()
    {
        if (ListRobots.Count > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i >= ListRobots.Count)
                    break;
                Robot rob = ListRobots[i].GetComponent<Robot>();

                if (rob)
                {
                    TimeLerp += SpeedCarpet;

                    if (TimeLerp > 1.0)
                        TimeLerp = 1.0f;
                    
                    if (rob.Imatricule == 0)
                        MovingAndDraging(Quart1Carpet,StartCarpet, i);
                    else if (rob.Imatricule == 1)
                        MovingAndDraging(Quart2Carpet, Quart1Carpet, i);
                    else if (rob.Imatricule == 2)
                        MovingAndDraging(EndCarpet, Quart2Carpet, i);
                    else if (rob.Imatricule == 3)
                        MovingAndDraging(Spawn, EndCarpet, i);

                    if (TimeLerp == 1.0f)
                        StopCarpet = true;
                }
            }
        }
    }

    void MovingAndDraging(Transform start, Transform end, int index)
    {
        DragAndDrop drag = ListRobots[index].GetComponent<DragAndDrop>();
        if (drag)
        {
            if (drag.IsDragging())
                drag.SetTempPos(Vector2.Lerp(start.position, end.position, TimeLerp));
                
            else
                ListRobots[index].transform.position = Vector2.Lerp(start.position, end.position, TimeLerp);
        }
    }

    void PauseCarpet()
    {
        CurrentTimeStop += Time.deltaTime;
        if (timeStopCarpet <= CurrentTimeStop)
        {
            CurrentTimeStop = 0;
            StopCarpet = false;
            for (int i = 0; i < 4; i++)
            {
                if (i < ListRobots.Count)
                {
                    Robot rob = ListRobots[i].GetComponent<Robot>();
                    if (rob)
                    {
                        rob.Imatricule -= 1;
                        if (rob.Imatricule == -1 && ListRobots.Count == 5)
                        {
                            ListRobots[4].GetComponent<Robot>().Imatricule = 3;
                            ListRobots.Remove(rob.gameObject);
                            Destroy(rob.gameObject);
                        }
                    }
                }
            } 
                
                
        }
        TimeLerp = 0;
    }

    public void Sorter(bool type, int imatricule)
    {
        if (type == true)
            choices[0]++;
        else 
            choices[1]++;

        for (int i = 0; i < 4; i++)
        {
            if (i < ListRobots.Count)
            {
                Robot rob = ListRobots[i].GetComponent<Robot>();
                if (rob.Imatricule == imatricule)
                {
                    ListRobots.Remove(rob.gameObject);
                    Destroy(rob.gameObject);
                }
            }
        }
        
        Debug.Log("Good:" + choices[0].ToString());
        Debug.Log("Bad:" + choices[1].ToString());
            
    }
}


