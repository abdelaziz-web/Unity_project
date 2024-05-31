using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class SnakeController : MonoBehaviour
{


    public float MoveSpeed = 5;
    public Vector3 RotateTo = new Vector3(0, 0, 1);
    public float SteerSpeed = 1000;
    public float BodySpeed = 5;

    public int Gap = 10;
    private Vector2Int input;
    public Vector3 direction = new Vector3();

    public float SpeedTimer = -1;
    public float ScoreTimer = -1;

    public float isFast = 1;

    public int isScore = 1;


    public GameObject BodyPrefab;
    public GameObject first_part;
    public GameObject Manager;


    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();


    void Start()
    {
        Grow_first();
        Grow_first();
    }
    void Update()
    {
        if (SpeedTimer > 0)
        {
            SpeedTimer -= Time.deltaTime;
        }
        else
        {
            isFast = 1;
        }
        if (ScoreTimer > 0)
        {
            ScoreTimer -= Time.deltaTime;
        }
        else
        {
            isScore = 1;
        }



        transform.position += transform.forward * MoveSpeed * Time.deltaTime * isFast;


        PositionsHistory.Insert(0, transform.position);


        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];


            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime * isFast;

            body.transform.LookAt(point);

            index++;
        }


        // funcion updating 
        InputManagement();
        RotateTowards(RotateTo, SteerSpeed, 5f);


    }

    private void GrowSnake()
    {

        if (BodyParts.Count > 0)
        {
            Vector3 lastPosition = BodyParts[BodyParts.Count - 1].transform.position;
            Vector3 lastForward = BodyParts[BodyParts.Count - 1].transform.forward;

            float offset = 0.3f;
            Vector3 newPosition = lastPosition - lastForward * offset;

            GameObject body = Instantiate(BodyPrefab, newPosition, Quaternion.identity);
            BodyParts.Add(body);
        }
        else
        {
            GameObject body = Instantiate(BodyPrefab, Vector3.zero, Quaternion.identity);
            BodyParts.Add(body);
        }
    }
    private void Grow_first()
    {
        GameObject body = Instantiate(first_part, Vector3.zero, Quaternion.identity);
        BodyParts.Add(body);
    }




    public void InputManagement()
    {
        float f_z = transform.forward.z;
        float f_x = transform.forward.x;
        if (Mathf.Abs(f_z) > Mathf.Abs(f_x))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotateTo = Vector3.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotateTo = Vector3.right;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RotateTo = Vector3.forward;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RotateTo = Vector3.back;
            }
        }
    }


    void RotateTowards(Vector3 targetDirection, float rotateSpeed = 90f, float rotationThreshold = 20f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);


        if (Quaternion.Angle(transform.rotation, targetRotation) > rotationThreshold)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = targetRotation;
        }
    }










    //Collision handlers

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("apple"))
        {
            Destroy(other.gameObject);
            ManagerSript managerScript = Manager.GetComponent<ManagerSript>();
            if (managerScript != null)
            {
                managerScript.AddScore(1 * isScore);
                managerScript.AddTime(3);
            }
            GrowSnake();
        }


        if (other.CompareTag("Body"))
        {
            SceneManager.LoadScene("Main");
        }

        if (other.CompareTag("Bolt"))
        {
            Destroy(other.gameObject);
            SpeedTimer = 10;
            isFast = 1.5f;
        }


        if (other.CompareTag("ScoreTimes2"))
        {
            Destroy(other.gameObject);
            ScoreTimer = 10;
            isScore = 2;
        }



        if (other.CompareTag("letter"))
        {
            Canvas canvas = other.GetComponentInChildren<Canvas>();
            TextMeshProUGUI textMeshPro = canvas.transform.Find("letter").GetComponent<TextMeshProUGUI>();
            char l = textMeshPro.text.ToCharArray()[0];
            Destroy(other.gameObject);


            ManagerSript managerScript = Manager.GetComponent<ManagerSript>();
            managerScript.CheckWordEaten(l);
            managerScript.AddScore(1 * isScore);
            managerScript.AddTime(4);
            GrowSnake();
        }


        if (other.CompareTag("Reducer"))
        {
            // Destroy the Reducer GameObject
            Destroy(other.gameObject);


            int partsToRemove = 4;
            
            while ( BodyParts.Count > 2   && partsToRemove>0)
            {
                partsToRemove-=1;
                int lastIndex = BodyParts.Count - 1;
                GameObject lastBodyPart = BodyParts[lastIndex];
                BodyParts.RemoveAt(lastIndex);
                Destroy(lastBodyPart);
            }
        }






    }





}