using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    //Character[] currentCharacters;
    public GameObject charactersParent;
    public Transform platformParent;
    //public Transform energyParent;
    public Transform elevationPlatform;
    public Transform screenDetect;
    public Transform UI;
    public Transform ControlParent;
    //public GameObject character_1;
    //public GameObject character_2;
    //public GameObject character_3;
    public int speed;

    /*
    Character[] heroes;
    Breakable[] breakables;
    bool timerReached;
    int direction;
    int points;
    float health;
    bool lift;
    bool jumpTimerReached = true;
    float liftTimer;

    bool drop;
    bool dropTimerReacher = true;
    float dropTimer;
    Vector3 currentFloorPos;
    */

    ResourceElemental elemental;
    PlayerStats character;
    RuneGenerator rGen;

    void Start()
    {
        Application.targetFrameRate = 30;
        // default platform switch direction is going up
        rGen = RuneGenerator.instance;

        /*
        direction = 0;
        points = 0;
        timerReached = true;
        lift = false;
        drop = false;
        breakables = platformParent.GetComponentsInChildren<Breakable>();
        */

        //heroes = charactersParent.GetComponentInChildren<Character>();
        
        character = charactersParent.GetComponentInChildren<PlayerStats>();
        //currentFloorPos.y = floor.transform.position.y;
        
    }

    public void SetElemental(int element) { elemental = (ResourceElemental)element; }
    public int GetElemental() { return (int)elemental; }

    //public int GetFloor() { return points; }

    // horizontal rune cutting
    /*
    public void Crush(int elemental)
    {
        for (int i = 0; i < breakables.Length; i++)
        {
            if ((int)breakables[i].element == elemental && breakables[i].vulnerable == BreakingType.Cut)
            {
                if (breakables[i].type == ResourceType.Energy)
                    character.CollectEnergy(elemental, breakables[i].SpawnLoots());
                else
                {
                    character.DeductEnergy(elemental);
                }
            }
        }
    }
    */

    // public void Boost() { character.GetBoost(); }

    // go up 1 platform
    /*
    public void LiftUp()
    {
        if(points < 2 && !drop) { lift = true; }
        elevationPlatform.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    // go down 1 platform
    public void DropDown()
    {
        if (points > -2 && !lift)
        {

            drop = true;
                //Debug.Log(character.GetCurrentGround().GetComponent<PlatformStatesSwitch>().ground.GetComponent<BoxCollider2D>().isTrigger);
                //character.GetCurrentGround().GetComponent<Collider2D>().isTrigger = true;
            character.GetCurrentGround().GetComponent<PlatformStatesSwitch>().ground.GetComponent<BoxCollider2D>().isTrigger = true;
                //elevationPlatform.GetComponent<BoxCollider2D>().isTrigger = false;

            //drop = false;
            //floor--;
        }
    }
    

    public void IncrementPoints(int count)
    {
        points += count;
    }
    */
    public int GetSpeed()
    {
        /*
        float sum = 0;
        heroes = charactersParent.GetComponentsInChildren<Character>();
        for (int i = 0; i < heroes.Length; i++)
        {
            sum += heroes[i].GetComponent<PlayerStats>().currentSpeed;
        }

        /*
        float sum = 0;
        sum += character_1.GetComponent<PlayerStats>().currentSpeed;
        sum += character_2.GetComponent<PlayerStats>().currentSpeed;
        sum += character_3.GetComponent<PlayerStats>().currentSpeed;
        

        team_speed = Mathf.FloorToInt(character.currentSpeed);
        */
        return speed;
    }

    /*
    public Vector3 GetPlayerPos()
    {
        return character.transform.position;
    }
    */

    /*
    public Vector3 GetPlayerPlatformPos()
    {
        //return character.GetCurrentGround().transform.position;
        return currentFloorPos;
    }
    */

    //public void SetSwitchDirection(int d) { direction = d; }

    public void PauseGame()
    {
        character.transform.GetComponent<Animator>().enabled = false;
        character.transform.GetComponent<PlayerStats>().enabled = false;
        character.transform.Find("animator").gameObject.SetActive(false);
        platformParent.GetComponent<ObstacleParallax>().enabled = false;

        // *** bring up pause menu //
    }

    public void Restart()
    {
        UI.Find("GameOver").gameObject.SetActive(false);

        character.transform.GetComponent<PlayerStats>().enabled = true;
        platformParent.GetComponent<ObstacleParallax>().enabled = true;

        platformParent.GetComponent<ObstacleParallax>().Restart();
        character.transform.GetComponent<Animator>().enabled = true;
        
        //character.transform.GetComponent<PlayerStats>().AddHealth(100);
        character.transform.Find("animator").gameObject.SetActive(true);
  
    }

    /*
    // Determine if character has landed on a platform
    bool TouchDown()
    {
        if (character.GetCurrentGround() != null && !character.GetCurrentGround().name.Contains("player"))
        {
            //character.GetCurrentGround().GetComponent<PlatformStatesSwitch>().SetCurrentPlatform();
            currentFloorPos = character.GetCurrentGround().position;
            SetPlatformPos(); return true;
        }
        else { return false; }
    }
    */

        /*
    // determine Y position of character's current platform
    void SetPlatformPos()
    {
        Vector3 pos = heroes[0].transform.position;
        pos.y -= 2f;
        elevationPlatform.transform.position = pos;
    }
    */

    void SetStatusBar()
    {
        int index = (int)elemental;
        character.transform.Find("animator").GetComponentInChildren<SpriteRenderer>().color = rGen.ReturnColor(index);
        //Debug.Log(character.transform.Find("animator").GetComponentInChildren<SpriteRenderer>().name);
    }
    /*
    void ActivatePlatformSwitch()
    {
        if(direction == 0) { LiftUp(); }
        else if(direction == 2) { DropDown(); }
    }
    */

    void StatusUpdate()
    {
        //string level;
        //if(floor > 0) { level = floor + "F"; }
        //else if(floor < 0) { level = "B" + -floor; }
        //else { level = "G"; }

        //UI.Find("point").transform.GetComponent<Text>().text = points.ToString();

        // mock up progress bar of stage
        int count = platformParent.GetComponent<ObstacleParallax>().GetPlatformPassed();
        if (count < 6 && count > 0)
        {
            string progress = "";
            for (int i = 0; i < count; i++)
            {
                progress += "- ";
            }
            UI.Find("progressBar").transform.GetComponent<Text>().text = count + " " + progress;
        }
        else { UI.Find("progressBar").transform.GetComponent<Text>().text = "BOSS"; }

        // mock up collected energy display
        float[] points = character.GetEnergyCollected();
        UI.Find("energyBar").transform.GetComponent<Text>().text =
            "Air: " + points[0] + "\nWater: " + points[1] + "\nEarth: " + points[2] + "\nFire: " + points[3];
    }

    /*
    void CurrentHealth()
    {
        
        //health = character.GetComponent<PlayerStats>().GetCurrentHealth();
        if(health < 1)
        {
            health = 0;
            PauseGame();
            UI.Find("GameOver").gameObject.SetActive(true);
        }
    }
    */

    void Update()
    {
        // update camera position in Y axis
        screenDetect.GetComponent<ScreenDetection>().SetScreenDetection(character.transform);
        // update platform spawner if new platform is needed
        platformParent.GetComponent<ObstacleParallax>().SetPlatformEnded(character.PlatformHasEnded());
        // update platform spawner whether breakable should spawn
        //platformParent.GetComponent<ObstacleParallax>().EnableBreakableSpawner(character.EnableBreakableSpawn());
        //energyParent.GetComponent<ObstacleParallax>().EnableBreakableSpawner(character.EnableBreakableSpawn());
        //CurrentHealth();
        StatusUpdate();
        SetStatusBar();

        //if (character.PlatformSwitch()) { ActivatePlatformSwitch(); }

        /*
        if (liftTimer > 1.5) {
            lift = false;
            points++;
            //platform.position = currentFloorPos;
            liftTimer = 0;
            elevationPlatform.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        if (lift && !drop)
        {
            //Debug.Log("lifting.");
            liftTimer += Time.deltaTime;
            elevationPlatform.transform.Translate(Vector2.up * 0.3f);
        }

        
        if (dropTimer > 0.5)
        {
            drop = false;
            points--;
            //platform.position = currentFloorPos;
            dropTimer = 0;
            elevationPlatform.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        if (!lift && drop)
        {
            dropTimer += Time.deltaTime;
            character.transform.position = Vector2.MoveTowards(new Vector2(character.transform.position.x, character.transform.position.y), new Vector2(character.transform.position.x + 1, character.transform.position.y - 5), 0.4f);
            elevationPlatform.transform.Translate(Vector2.down * 0.3f);
        }
        

        // prepare platform for the next lift
        if(!lift && !drop)
        {
            elevationPlatform.GetComponent<BoxCollider2D>().isTrigger = true;
            TouchDown();
            //Debug.Log("has character landed? " + TouchDown());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(elemental);
        }
        //Debug.Log("Platform status: " + character.PlantformHasEnded());
        */
        

        //SetPlatformPos();

        //if(character.GetCurrentGround() != null)
        //    currentFloorPos = character.GetCurrentGround().position + (Vector3.down * 0.1f);
        //get_team_speed();

        //if (!timerReached) { timer += Time.deltaTime; }
        //if (timer > 0.4) { heroes[0].GetComponent<Rigidbody2D>().gravityScale = 11; timerReached = true; timer = 0; }
    }

    public enum ResourceElemental { Air, Water, Earth, Fire, ALLTIME } 
    public enum ResourceType { Energy, Material }

    //public enum BreakingType { Cut, Crush, Pull }

}
