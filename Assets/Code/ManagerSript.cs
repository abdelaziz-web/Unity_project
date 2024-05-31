using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class ManagerSript : MonoBehaviour
{
public string[] words = {
    // Easy words (3 letters)
    "BAT", "BIT", "BOG", "BUS", "CAT", "COW", "COP", "DOT", "DOG", "DAD", 
    "EGG", "FOG", "FUN", "GOT", "HAT", "HIT", "HOT", "JOG", "LEG", "LOG", 
    "MAT", "MOM", "MOP", "PIT", "POP", "POT", "RAT", "RUN", "SAT", "SIT", 
    "SUN", "TOP", "TAP", "YAM", "ZOO",

    // Medium words (4-5 letters)
    "BALD", "BARK", "BIRD", "BITE", "BOOK", "BRUSH", "BUDGE", "CAGE", "COLD", 
    "COW", "COOK", "COPY", "CRAM", "CUSP", "DIVE", "DROP", "DUCK", "FELT", 
    "FLIP", "FOLD", "GOLD", "GRIP", "HELP", "HOLD", "JUMP", "KITE", "LAMP", 
    "LIFT", "LOGO", "MARK", "MEET", "MELT", "MOVE", "MUST", "NUDGE", "PAGE", 
    "PUSH", "PULL", "RUSH", "SING", "SLIP", "STOP", "SWIM", "TEST", "TREE", 
    "TRIP", "WALK", "WASH", "WELD", "WIND", "YELL",

    // Hard words (6-8 letters)
    "CABINET", "CAMERA", "CARROT", "CIRCLE", "CLINIC", "COMPUTER", "DELIGHT", 
    "DENTIST", "EAGER", "ENGINEER", "ENVELOPE", "EXCITED", "FRIEND", "GLOWING", 
    "HEDGE", "ILLUMINATED", "INSIGHT", "JOURNALIST", "KNOWLEDGE", "LAWYER", 
    "LIBRARY", "MAGAZINE", "MEDICINE", "PRACTICE", "PREMIUM", "RESPONSE", 
    "SENSITIVE", "SIGNIFICANT", "STUDY", "SUPPLY", "TEACHER", "TOGETHER", 
    "UNDERSTANDING", "UNIQUE", "VEHICLE", "VILLAGE", "WEEKEND", "WILDERNESS", 
    "YESTERDAY",

    // Very Hard words (9-11 letters)
    "ACCORDINGLY", "ADDITIONALLY", "ADJUSTMENT", "AGITATED", "ALARM", "ALLEGATION", 
    "ALLEGE", "ALLEGE", "ALMOST", "ALTERNATE", "AMENDMENT", "AMIDST", "ALLEGATION", 
    "AMONG", "ANALYSIS", "ANTHONY", "ANTICIPATION", "ANTIQUE", "ANYWAY", "APARTMENT", 
    "APPROACHES", "APPROVE", "AWAKENED", "BLUSH", "BRIGHT", "BUTTON", "COMPUTER", 
    "DIFFICULT", "EVIDENCE", "EXCITED", "FRIEND", "HEDGE", "HORRIFIED", "INSIGHT", 
    "INVESTIGATE", "JOURNALIST", "KNOWLEDGE", "LAWYER", "LIBRARY", "LUMINOSITY", 
    "MAGAZINE", "MEDICINE", "NEIGHBORHOOD", "PHOTOGRAPHER", "POSSIBILITY", "PRACTICE", 
    "PREDICTABLE", "PREMIUM", "PROCEDURE", "RECOGNIZE", "REGULATE", "RELATIONSHIP", 
    "RESPONSE", "SCIENTIST", "SENSITIVE", "SIGNIFICANT", "SUPPLY", "TEACHER", 
    "TOGETHER", "UNDERSTANDING", "UNIQUE", "VEHICLE", "VILLAGE", "WEEKEND", 
    "WILDERNESS", "YESTERDAY",

    // Extreme words (12-15 letters)
    "ACTUALIZATION", "ADVENTURE", "ALPHABETICAL", "APPROPRIATION", "ARCHAEOLOGICAL", 
    "ASTRONOMICAL", "CONSCIENTIOUS", "DECONSTRUCTION", "EXPERIMENTATION", "PHILOSOPHICAL", 
    "PROFESSIONAL", "PSYCHOLOGICAL", "QUANTIFICATION", "QUESTIONNAIRE", "RECONSIDERATION", 
    "SUPERCALIFRAGILISTICEXPIALIDOCIOUS", "TRANSFORMATION", "UNDERSTANDABILITY", 
    "UNCONVENTIONAL", "UNOBJECTIONABLE","Acclimatizing", "Accumulative", "Acknowledgment", "Administrations", "Affectionately",
    "Alphabetically", "Appreciatively", "Architecturally", "Atmospherically", "Collaborations",
    "Communications", "Complications", "Constructions", "Contemplations", "Contributions",
    "Coordinations", "Demonstrations", "Determinations", "Disconnections", "Disorientations",
    "Electromagnets", "Encouragements", "Entertainingly", "Establishments", "Experimentally",
    "Facilitations", "Fundamentality", "Generationally", "Implementations", "Inspirationally",
    "Interpretations", "Misunderstands", "Notifications", "Organizational", "Philosophically",
    "Professionalism", "Psychologically", "Reconsideration", "Supercalifragilisticexpialidocious",
    "Transformations"

};


    private List<GameObject> words_3d = new List<GameObject>();
    public string word = "";

    public char letter_to_be_eaten;
    public char[] letters;

    public GameObject Snake;
    public float destroyBoostDelay = 15f;
    public GameObject Obstacle;
    private GameObject[] boosts;

    public GameObject Bolt;
    public GameObject ReducerSnake;

    public GameObject Letter;

    public GameObject ScoreTimes2;

    public GameObject word_display;
    public int level=1;






    public float boost_spawn_time = 2;


    public float obstacle_spawn_time = 3;


    public GameObject TimerText;
    public GameObject ScoreText;
    public GameObject leveltext;
    public float score = 0;
    public int num_words = 0;
    public float timer = 20f;
    public int num_letters = 0;

    public Text TextTimerText;
    public Text TextScoreText;
    public Text textlevel ;
    public Text TextWordText;
    void Start()
    {
        timer = 20f;
        textlevel = leveltext.GetComponent<Text>();
        TextTimerText = TimerText.GetComponent<Text>();
        TextScoreText = ScoreText.GetComponent<Text>();
        TextWordText = word_display.GetComponent<Text>();
        word_spawner(word);
        InstantiatePrefab(ReducerSnake);
    }


    void Update()
    {

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            SceneManager.LoadScene("Main");
        }

        UpdateText();



        boost_spawner();
        Obstacle_spawner();
        word_spawner(word);




    }




    public void AddScore(int x)
    {
        score += x;
    }
    public void AddTime(int t)
    {
        timer += t;
    }
    public void UpdateText()
    {
        TextScoreText.text = score.ToString();
        TextTimerText.text = Mathf.CeilToInt(timer).ToString() + " s";
        TextWordText.text = word;
         
        level = SetLevelBasedOnScore((int) score);
        textlevel.text = level.ToString();
    }



    private bool IsOccupied(Vector3 pos, Vector3 generated_pos, float radius)
    {

        float distance = Vector3.Distance(generated_pos, pos);
        if (distance > radius)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    private Vector3 GeneratePosition()
    {
        float edge = 11f;
        float randomX = Random.Range(-edge, edge);
        float randomZ = Random.Range(-edge, edge);


        Vector3 randomPosition = new Vector3(randomX, 0.5f, randomZ);

        return randomPosition;
    }



    public GameObject InstantiatePrefab(GameObject prefabToInstantiate)
    {

        if (prefabToInstantiate != null)
        {
            Transform head_transform = Snake.transform.GetChild(0);
            Vector3 head_pos = head_transform.position;

            Vector3 pos = GeneratePosition();
            while (IsOccupied(head_pos, pos, 0.12f))
            {
                pos = GeneratePosition();
            }

            GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, pos, Quaternion.identity);
            instantiatedPrefab.transform.position = pos;
            return instantiatedPrefab;

        }
        else
        {
            Debug.LogWarning("Prefab to instantiate is null!");
            return null;
        }
    }


    void Obstacle_spawner()
    {
        if (obstacle_spawn_time > 0)
        {
            obstacle_spawn_time -= Time.deltaTime;

            if (obstacle_spawn_time <= 0)
            {
                GameObject O=InstantiatePrefab(Obstacle);
                Destroy(O, destroyBoostDelay*4);


                obstacle_spawn_time = (int)Random.Range(10, 20);
            }
        }

    }


    void boost_spawner()
    {
        if (boost_spawn_time > 0)
        {
            boost_spawn_time -= Time.deltaTime;

            if (boost_spawn_time <= 0)
            {

                GameObject prefabToSpawn = Random.value < 0.5f ? ReducerSnake : (Random.value < 0.5f ? ScoreTimes2 : Bolt);


                GameObject O=InstantiatePrefab(prefabToSpawn);
                Destroy(O, destroyBoostDelay);


                boost_spawn_time = (int)Random.Range(15, 250);
            }
        }
    }

    






    void word_spawner(string w)
    {
        GameObject prefabToSpawn = Letter;
        foreach(var word in words_3d ){
            Destroy(word) ;
        }
        if (w.Length == 0)
        {

            int range;

            if(level == 0)
            {
                range = 0;
            }

            else if(level == 1)
            {
                range = 30;
            }

            else if(level == 2)
            {
                range = 60;
            }
                        
            else if(level == 4)
            {
                range = 70;
            }

            else if (level == 5)
            {
                range = words.Length - 40;
            }

            else
            {
                range = words.Length - 20;
            }     


            word = words[Random.Range(range, words.Length)];
          
          

            letters = word.ToCharArray();
            letter_to_be_eaten = letters[0];

            foreach (var letter in letters)
            {
                GameObject letterObject = InstantiatePrefab(prefabToSpawn);
                Canvas canvas = letterObject.GetComponentInChildren<Canvas>();
                if (canvas != null)
                {
                    TextMeshProUGUI textMeshPro = canvas.transform.Find("letter").GetComponent<TextMeshProUGUI>();
                    if (textMeshPro != null)
                    {
                        textMeshPro.text = letter.ToString();
                    }
                }
            }
        }
    }


    public void CheckWordEaten(char l)
    {


        if (l == letter_to_be_eaten)
        {
                word = word.Substring(1);

                     
                letters = word.ToCharArray();

            if(letters.Length == 0)
            {

            }
            else
            {
                letter_to_be_eaten = letters[0];
                Debug.Log("Letter eaten! New word: " + word);

            }


        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    int  SetLevelBasedOnScore(int Score)
    {
        switch (Score)
        {
            case int n when (n >= 0 && n <= 5):
               return level = 1;
                
            case int n when (n >= 6 && n <= 10):
               return level = 2;
              
            case int n when (n >= 11 && n <= 15):
               return level = 3;
                
            case int n when (n >= 16 && n <= 20):
               return level = 4;
                      
               
            default:
              return  level = 5;
                
    }

    }







}
