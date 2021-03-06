using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public float speed = 5.0f;
    public GameObject[] bullets = new GameObject[4];  //Red Green Blue White Bullets
    [SerializeField] public float yUpperBound = 3.15f, yLowerBound = -4;
    [SerializeField] public float xPos = -7.0f;
    [SerializeField] public float impactTime = 0.5F;
    [SerializeField] private float nextFire = 0.0F;
    [SerializeField] private float placeX = 0.05F;
    public int life = 3;
    public int curLife { get; private set; }
    private int score = 0; //Kill a obstacle +2   False color spawn -1  
    public TextMeshProUGUI textScore;

    public bool superPowerWhite = false;


    // Start is called before the first frame update
    private void Awake()
    {
        transform.position = new Vector3(xPos, 0.0f, 0.0f);
        curLife = life;
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(0.0f, 1.0f, 0.0f);
        transform.Translate(direction * speed * Time.deltaTime * input);
        GetComponent<Animator>().SetBool("Run", input != 0);

        if(!superPowerWhite)
        {
            if (Input.GetKeyDown(KeyCode.Z) && Time.time > nextFire)
            {                                               //Spawns RED bullet with Z
                nextFire = Time.time + impactTime;
                Instantiate(bullets[0], new Vector3(gameObject.transform.position.x + placeX, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
                GetComponent<Animator>().SetTrigger("attack");
            }
            if (Input.GetKeyDown(KeyCode.X) && Time.time > nextFire)
            {                                               //Spawns GREEN bullet with X
                nextFire = Time.time + impactTime;
                Instantiate(bullets[1], new Vector3(gameObject.transform.position.x + placeX, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);;
                GetComponent<Animator>().SetTrigger("attack");
            }
            if (Input.GetKeyDown(KeyCode.C) && Time.time > nextFire)
            {                                                //Spawns BLUE bullet with C
                nextFire = Time.time + impactTime;
                Instantiate(bullets[2], new Vector3(gameObject.transform.position.x + placeX, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
                GetComponent<Animator>().SetTrigger("attack");
            }
        }
        else
        {
            if ( (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Z) ) && Time.time > nextFire)
            {                                             
                nextFire = Time.time + impactTime;
                Instantiate(bullets[3], new Vector3(gameObject.transform.position.x + placeX, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
                GetComponent<Animator>().SetTrigger("attack");
            }
        }

        //Prevent player to cross y bounds
        if (transform.position.y < yLowerBound)
        {
            transform.position = new Vector3(xPos, yLowerBound, 0);
        }
        if(transform.position.y > yUpperBound)
        {
            transform.position = new Vector3(xPos, yUpperBound, 0);
        }
    }

    public void DecreaseLife()
    {
        if (curLife != 0)
        {
            curLife = Mathf.Clamp(curLife - 1, 0, life);
            if (curLife > 0)
                GetComponent<Animator>().SetTrigger("hurt");
            
        }

        if (curLife == 0){
            
            GetComponent<Animator>().SetTrigger("die");
            CallMe();

        }
        
    }
    void CallMe()
    {
        // Invoke("MethodName", Delay seconds as float);
        Invoke("CallMeWithWait", 0.7f);
    }

    void CallMeWithWait()
    {
        Time.timeScale = 0;
        GetComponent<DeadHandler>().HandleDeath();

        Debug.Log("Game is OVER!");
    }

    public void IncreaseLife()
    {
        curLife++;
    }

    public void EnemyDestroyed()
    {
        score += 2;
        UpdateScore();
    }

    public void FalseColor()
    {
        score--;
        UpdateScore();
    }

    //Updates score on the screen.
    private void UpdateScore()
    {
        textScore.text = "Score:" + score;
    }
}