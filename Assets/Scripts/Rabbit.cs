using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public enum Gender { Male, Female };
    public Gender gender;
    private Rigidbody rb;
    public bool noAI = false;

    public float lifetime;
    [SerializeField] public Gene[] genes;

    public string state;

    float sight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // when a rabbit is created, the first frame, decide the gender of the rabbit by flipping a coin
        int coinflip = UnityEngine.Random.Range(1, 3);
        if (coinflip == 1) gender = Gender.Male;
        else gender = Gender.Female;



        position = new Vector2(transform.position.x, transform.position.z);
        RandomPatrolPoint();
    }

    void OnDestroy()
    {
        // when the rabbit object is destroyed, remove it from the list of rabbits
        RabbitManager.rabbits.Remove(this);
    }

    // run once when this rabbit is created to pass down the genes of the parents
    public void NewRabbit(Gene[] newGenes)
    {
        genes = newGenes;
    }

    void Birth(Rabbit otherParent)
    {
        // calculate child's genes
        List<Gene> newGenes = new List<Gene>();

        for (int i = 0; i < genes.Length; i++)
        {
            for (int J = 0; J < otherParent.genes.Length; J++)
            {
                if (genes[i].name == otherParent.genes[J].name)
                {
                    // copy the parents gene, making sure that teh correct types are passed on
                    Gene g = Gene.Copy(genes[i]);
                    // add both parents modifiers together
                    g.SetModifier(genes[i].GetModifier() + otherParent.genes[J].GetModifier());
                    //add this to the list of genes that will be passed onto the child
                    newGenes.Add(g);
                }
            }
        }
        Gene[] childGenes = newGenes.ToArray();

        // create new rabbit with these genes
        RabbitManager.CreateRabbit(childGenes,
            this.transform.position + new Vector3(5, 0, 5));
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (gender == Gender.Female)
            {
                Rabbit r = RabbitManager.RandomRabbit(RabbitManager.GenderSearch.Male);
                Birth(r);
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            RandomPatrolPoint();
        }
        

        foreach(Gene g in genes)
        {
            g.Update();
        }
    }
    private void FixedUpdate()
    {
        if (!noAI)
            AI();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sight);
    }

    /* The majority of the calcuations are done on a 2 dimentional flat plane
     * this function converts a Vector3 to a vector2, by dropping the y component
     */
    Vector2 ToVector2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

///////////////////////////////////////////////////////////////////////
//                   MOVEMENT / AI
///////////////////////////////////////////////////////////////////////

    Vector3 patrolPoint;
    float interactionRadius = 5.0f;

    void RandomPatrolPoint()
    {

        float r = 20;
        Vector3 randPos;
        do
        {
            float randX = UnityEngine.Random.Range(-r, r);
            float randZ = UnityEngine.Random.Range(-r, r);
            randPos = new Vector3(randX, 40, randZ);
        }
        while (!Eco.isValidGround(transform.position +  randPos));

        patrolPoint = new Vector3(  transform.position.x + randPos.x,
                                    transform.position.y,
                                    transform.position.z + randPos.z);
    }


    float timeToDrink = 2.5f;
    float drinkTimer = 0;
    float timeToMate = 5.5f;
    float mateTimer = 0;

    void AI()
    {
        float topPercent = 0;
        string topName = "";

        // loop through all the genes. whichever gene has the highest value defines the state of the rabbit
        foreach(Gene g in genes)
        {
            // do not factor in genes like eyesight and movement speed
            if (g.decisionFactor)
            {

                float percent = g.value / g.cap;
                if (percent > topPercent)
                {
                    topPercent = percent;
                    topName = g.name;
                }
            }
        }
        sight = GeneManager.GetGeneFromName(genes, "Eyesight").value;
        Collider[] colliders;

        // decide the current state based on genes
        switch (topName)
        {
            case "Hunger":
                state = "Searching For Food";

                GameObject food = null;
                // search all colliders in range for a gameobject with tag "Food"
                colliders =  Physics.OverlapSphere(transform.position, sight);
                foreach(Collider c in colliders)
                {
                    if (c.gameObject.tag == "Food")
                    {
                        food = c.gameObject;
                        break;
                    }
                }
                // if no food can be found, patrol normally
                if(food == null) Patrolling();
                // if food is found, go towards it
                else
                {
                    Movement(food.transform.position);
                    //when near food, eat it and reset hunger
                    if(Vector2.Distance(ToVector2(transform.position), ToVector2(food.transform.position)) < interactionRadius)
                    {
                        food.SendMessage("Eat");
                        GeneManager.GetGeneFromName(genes, "Hunger").value = 0;
                    }
                }



                break;
            case "Thirst":
                state = "Searching For Water";

                GameObject water = null;
                // search all colliders in range for a gameobject with tag "WaterBorder"
                colliders = Physics.OverlapSphere(transform.position, sight);
                foreach (Collider c in colliders)
                {
                    if (c.gameObject.tag == "WaterBorder")
                    {
                        water = c.gameObject;
                        break;
                    }
                }
                // if no food can be found, patrol normally
                if (water == null) Patrolling();
                // if water is found, go towards it
                else
                {
                    Movement(water.transform.position);
                    //when near food, eat it and reset hunger
                    if (Vector2.Distance(ToVector2(transform.position), ToVector2(water.transform.position)) < interactionRadius)
                    {
                        state = "drinking";

                        drinkTimer += Time.deltaTime;
                        if(drinkTimer > timeToDrink)
                        {
                            drinkTimer = 0;
                            //reset thirst
                            GeneManager.GetGeneFromName(genes, "Thirst").value = 0;
                        }
                    }
                }
                break;

            case "reproductiveUrge":
                state = "Searching For a mate";
                
                GameObject mate = null;
                // search all colliders in range for a gameobject with tag "Rabbit"
                colliders = Physics.OverlapSphere(transform.position, sight);
                foreach (Collider c in colliders)
                {
                    if (c.gameObject.tag == "Rabbit")
                    {
                        Rabbit r = c.GetComponent<Rabbit>();
                        // if the potential mate is also searching for a partner,
                        // and is of the opposite gender
                        if(    r.state == "Searching For a mate"
                            && r.gender != gender)
                        {
                            mate = c.gameObject;
                            break;
                        }
                    }
                }
                state = "Found a mate";
                // if no mate can be found, patrol normally
                if (mate == null) Patrolling();
                // if a mate is found, go towards it
                else
                {
                    Movement(mate.transform.position);
                    //when near  mate
                    if (Vector2.Distance(ToVector2(transform.position), ToVector2(mate.transform.position)) < interactionRadius)
                    {
                        state = "Mating";

                        // begin mating
                        mateTimer += Time.deltaTime;
                        if (mateTimer > timeToMate)
                        {
                            mateTimer = 0;

                            // once mating is complete, the male will send a reference to his class to the female so the genes can be passed on
                            if (gender == Gender.Male)
                                mate.GetComponent<Rabbit>().Birth(this);

                            //reset thirst
                            GeneManager.GetGeneFromName(genes, "ReproductiveUrge").value = 0;
                        }
                    }
                }
                break;


            default:
                Debug.Log("no switch statement for: " + topName);
                break;

                //case "Fear":
                //    state = "Running From Fox";
                //    break;
                //default:
                //    state = "no state";
                //    Patrolling();
                //    break;
        }

    }



    [SerializeField] float speed;
    [SerializeField] float steerStrength;
    Vector2 velocity;
    Vector2 position;
    void Patrolling()
    {
        if (Vector2.Distance(ToVector2(this.transform.position), ToVector2(patrolPoint)) < interactionRadius)
        {
            RandomPatrolPoint();
        }
        Vector3 vectorToTarget = (patrolPoint - this.transform.position);

        Debug.DrawRay(transform.position, new Vector3(vectorToTarget.x, 0, vectorToTarget.z));
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        Movement2D(ToVector2(vectorToTarget));
    }
    void Movement(Vector3 target)
    {
        Vector3 vectorToTarget = (target - this.transform.position);
        Debug.DrawRay(transform.position, new Vector3(vectorToTarget.x, 0, vectorToTarget.z));
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        Movement2D(ToVector2(vectorToTarget));
    }
    void Movement2D(Vector2 directionOfMovement)
    {

        Vector2 desiredVelocity = directionOfMovement * speed;
        Vector2 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength;
        Vector2 acceleration = Vector2.ClampMagnitude(desiredSteeringForce, steerStrength) / 1;

        velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, speed);
        position += velocity * Time.deltaTime;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.SetPositionAndRotation(new Vector3(position.x, transform.position.y, position.y), Quaternion.Euler(0, -angle + 90, 0));

    }
}

