using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
  
    
    private const float CAMERA_ORTHO_SIZE = 50F;
    private const float PIPE_WIDTH = 6F;
    private const float PIPE_HEAD_HEIGHT = 4F;
    private const float PIPE_MOVE_SPEED = 15F;
    private const float PIPE_DESTROY_XPOSITION = -100F;
    private const float PIPE_SPAWN_XPOSITION = +100F;
    private const float GROUND_DESTROY_XPOSITION = -200F;
    private const float GROUND_SPAWN_XPOSITION = +100F;
    private const float CLOUD_DESTROY_XPOSITION = -100F;
    private const float CLOUD_SPAWN_XPOSITION = +100F;
    private const float BIRD_X_POSITION = 0F;

    private static Level inastance;

    public static Level GetInstance()
    {
        return inastance;
    }
    private List<Transform> groundList;
    private List<Transform> cloudList;
    private List<Pipe> pipeList;
    private int pipePassedCount;
    private int pipesSpawned;
    private float PipeSpawnTimer;
    private float PipeSpawnTimerMax;
    private float gapSize;
    private State state;
    public enum Difficulty
    {
        Easy,
        Meduim,
        Hard,
        Impossible,

    }
    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }
    private void Awake()
    {
        inastance = this;
        pipeList = new List<Pipe>();
        SpawnInitialGround();
        SpawnInitialCloud();
        PipeSpawnTimerMax = 3f;//المسافة بين كل ابنوبة و التانيه
        /*gapSize = 50f;*///المسافة بين الانبوبة اللي فوق و اللي تحت
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }
    private void Start()
    {
        //StartCoroutine(SpawningFlowers());
        Bird.GetInstance().onDied += Bird_OnDied;
        Bird.GetInstance().onStartPlaying += Bird_OnStartPlaying;
    }
    private void Bird_OnStartPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }
    private void Bird_OnDied(object sender,System.EventArgs e)
    {
        state = State.BirdDead;
    }
    private void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
            HandleGround();
            HandleClouds();
        }
        
    }
    private void SpawnInitialCloud()
    {
        cloudList = new List<Transform>();
        Transform cloudTransform;
        float cloudY = 30f;
        float cloudX = 0f;
        float cloudWidth = 18.5f;
        cloudTransform = Instantiate(GameAssets.GetInstance().cloud, new Vector3(cloudX, cloudY, 0), Quaternion.identity);
        cloudList.Add(cloudTransform);
        cloudTransform = Instantiate(GameAssets.GetInstance().cloud, new Vector3(cloudX-38.4f, cloudY+4.6f, 0), Quaternion.identity);
        cloudList.Add(cloudTransform);
        cloudTransform = Instantiate(GameAssets.GetInstance().cloud, new Vector3(cloudX + 29.2f, cloudY + 5.6f, 0), Quaternion.identity);
        cloudList.Add(cloudTransform);
    }
    private void HandleClouds()
    {
        foreach (Transform cloudTransform in cloudList)
        {
            cloudTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            float rightMostXPosition = -100f;
            if (cloudTransform.position.x < CLOUD_DESTROY_XPOSITION)
            {
                for (int i = 0; i < cloudList.Count; i++)
                {
                    if (cloudList[i].position.x > rightMostXPosition)
                    {
                        rightMostXPosition = cloudList[i].position.x;
                    }
                }
                float cloudWidth = 120f;
                cloudTransform.position = new Vector3(rightMostXPosition + cloudWidth, cloudTransform.position.y, cloudTransform.position.z);

            }
        }
    }
    private void SpawnInitialGround()
    {
        groundList = new List<Transform>();
        Transform groundTransform;
        float groundY = -47.5f;
        float groundWidth = 192f;
        groundTransform = Instantiate(GameAssets.GetInstance().ground, new Vector3(0, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().ground, new Vector3(groundWidth, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().ground, new Vector3(groundWidth*2f, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
    }
    private void HandleGround()
    {
        foreach(Transform groundTransform in groundList)
        {
            groundTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            float rightMostXPosition = -100f;
            if (groundTransform.position.x < GROUND_DESTROY_XPOSITION)
            {
                for(int i = 0; i < groundList.Count; i++)
                {
                    if(groundList[i].position.x > rightMostXPosition)
                    {
                        rightMostXPosition = groundList[i].position.x;
                    }
                }
                float groundWidth = 192f;
                groundTransform.position = new Vector3(rightMostXPosition + groundWidth, groundTransform.position.y, groundTransform.position.z);

            }
        }
    }
    private void HandlePipeSpawning()
    {
        PipeSpawnTimer -= Time.deltaTime;
        if (PipeSpawnTimer < 0)
        {
            PipeSpawnTimer += PipeSpawnTimerMax;
            float heightEdgeLimit = 10f;
            float minHeight = gapSize * 0.5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);
            CreateGapPips(height, gapSize, PIPE_SPAWN_XPOSITION);
        }
    }
    private void HandlePipeMovement()
    {
        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isToTheRightBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if(isToTheRightBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsButton())
            {
                pipePassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.ScoreSound);
                
            }
            if (pipe.GetXPosition() < PIPE_DESTROY_XPOSITION)
             {
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
             }
            
        }
    }
    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 55f;
                PipeSpawnTimerMax = 2.5f;
                break;
            case Difficulty.Meduim:
                gapSize = 50f;
                PipeSpawnTimerMax = 2f;
                break;
            case Difficulty.Hard:
                gapSize = 45f;
                PipeSpawnTimerMax = 1.5f;
                break;
            case Difficulty.Impossible:
                gapSize = 35f;
                PipeSpawnTimerMax = 1.3f;
                break;

        }

    }
    public int GetPipeSpawned()
    {
        return pipesSpawned;
    }
    public int GetPipePassedCount()
    {
        return pipePassedCount;
    }
    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) return Difficulty.Impossible;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Meduim;
        return Difficulty.Easy;
    }
    
    private void CreateGapPips(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * 0.5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * 0.5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    //test flowers
    
    private void CreatePipe(float height,float xPosition,bool createButton)
    {
        
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYposition;
        if (createButton)
        {
            pipeHeadYposition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * 0.5f;
        }
        else
        {
            pipeHeadYposition = +CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * 0.5f;
        }
        pipeHead.position = new Vector2(xPosition, pipeHeadYposition);
        //pipeList.Add(pipeHead);


        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);

        //test flower

        float pipeBodyYposition;
        if (createButton)
        {
            pipeBodyYposition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYposition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector2(xPosition, pipeBodyYposition);
        //pipeList.Add(pipeBody);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyboxCollider2D = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyboxCollider2D.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyboxCollider2D.offset = new Vector2(0f, height * 0.5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody, createButton);
        pipeList.Add(pipe);
    }
    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool createButton;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform,bool createButton)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.createButton = createButton;
        }
        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }
        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }
        public bool IsButton()
        {
            return createButton;
        }
        public void DestroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }
    }
}
