using UnityEngine;
using System.Collections;
using FreeflowCombatSpace;
using CMF;
using DG.Tweening;

//[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class FreeflowCombat : MonoBehaviour
{
    [Tooltip("Automatically get game camera on start.")]
    public bool autoGetCamera = true;
    [Tooltip("Set the game camera.")]
    public new Transform camera;
    [Tooltip("Set the value of your input (system) X-axis without normalizing. This should be done every frame through a script.")]
    public float xInput;
    [Tooltip("Set the value of the input (system) Y-axis without normalizing. This should be done every frame through a script.")]
    public float yInput;
    [Tooltip("The layers of enemies.")]
    public LayerMask enemyLayers;
    [Tooltip("The radius that detects surrounding enemies and can traverse to them.")]
    public float detectionRadius = 10f;
    [Tooltip("If set to true will show the detection radius in scene view as a yellow wire sphere.")]
    public bool showDetectionRadius = true;
    [Tooltip("Accuracy size of finding the next target. The bigger it is, the easier it is to find your next enemy but won't be as accurate relative to direction. This is viewed as a green wire sphere in scene view. Rule of thumb: the sphere size should look atleast half as big as your enemies.")]
    public float accuracySize = 2f;
    [Tooltip("Turn the accuracy sphere on or off in scene view.")]
    public bool showAccuracy = true;
    [Tooltip("The time of isTraversing, moving from one enemy to another.")]
    public float traversalTime = 5f;

    [Tooltip("The state name for your idle animation.")]
    public string idleAnimName;
    [Tooltip("The speed of transition from whatever animation currently playing to idle animation.")]
    public float idleAnimTSpeed = 0.3f;

    [Tooltip("Set all your attack animation names and their attack distance.")]
    public AttackAnimations[] attackAnimations;
    [Tooltip("Attack Animations Transition Speed - set the speed of the animation transition.")]
    public float attackAnimsTSpeed = 0.3f;
    [Tooltip("If set to true, on each attack a random attack animation will be chosen and played. if set to false, animations will be played linearly one after another in a looping fashion.")]
    public bool randomizeAttackAnim = false;

    [Tooltip("If set to true, animations will play during traversal, when attack distance is reached then the attack animations will play. If set to false attack animations will during traversal.")]
    public bool useTraversalAnimations = false;
    [Tooltip("Play the traversal animations if the distance between player and enemy is bigger or equal to this.")]
    public float applyTraversalAnimDistance;
    [Tooltip("Set all your traversal animations. Traversal animations are the animations played when you're moving from enemy to another.")]
    public TraversalAnimations[] traversalAnimations;
    [Tooltip("Traversal Animations Transition Speed - set the speed of the animation transition.")]
    public float traversalAnimsTSpeed = 0.3f;
    [Tooltip("If set to true, on each traversal a random traversal animation will be chosen and played. If set to false, animations will be played linearly one after another in a looping fashion.")]
    public bool randomizeTraversalAnim = false;
    [Tooltip("If set to true, the player will traverse to the enemy while maintaining it's current Y position value, if set to false the player will traverse to where the FreeFlowCombatEnemy script Y position is.")]
    public bool maintainYPosition = true;

    [Tooltip("Add the scripts you want to disable when attacking and they will be automatically re-enabled. You will most probably have to add your movement script.")]
    public MonoBehaviour[] scriptsToDisable;
    public FreeflowCombatEnemy currentTarget { get; set; }
    public bool isTraversing { get; set; }
    public bool isAttacking { get; set; }

    public Animator anim;
    AnimationManager animManager;
    //CharacterController controller;

    int attackAnimIndex = -1;
    int traversalAnimIndex = -1;
    float currentHitDist;
    bool reachedAttackPosition = false;
    bool attacked = false;
    bool defaultRootMotion;

    FreeflowCombatEnemy nextEnemy;
    float timer = 0f;

    AdvancedWalkerController controller;

    public static FreeflowCombat instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.Normal;
        animManager = new AnimationManager(anim);

        controller = GetComponent<AdvancedWalkerController>();
        //defaultRootMotion = anim.applyRootMotion;

        if (autoGetCamera) camera = Camera.main.transform;
        xInput = CharacterDefaultInput.instance.MovementInput.x;
        yInput = CharacterDefaultInput.instance.MovementInput.y;
    }

    void OnValidate()
    {
        if (GetComponent<Animator>())
            GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
    }

    // draw detection radius wire sphere
    void OnDrawGizmosSelected()
    {
        if (showDetectionRadius) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        if (showAccuracy) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, accuracySize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterDefaultInput.instance.Shoot_Right_Input)
        {
            Attack();

        }



        xInput = CharacterDefaultInput.instance.MovementInput.x;
        yInput = CharacterDefaultInput.instance.MovementInput.y;
        // player reached attack position
        if (reachedAttackPosition) {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99) {
                StopAttacking();
            }
        }

        if (nextEnemy != null && attacked) {
            timer += Time.deltaTime;
            if (timer >= 0.3f) {
                nextEnemy = null;
                timer = 0f;
            }
        }
    }

    // public method for attacking
    public void Attack()
    {
        var inputDirection = (camera.forward * yInput) + (camera.right * xInput);
        inputDirection = inputDirection.normalized;
        inputDirection = new Vector3(inputDirection.x, 0f, inputDirection.z);

        AdvancedWalkerController.instance.CC_Rigid.isKinematic = true;

        if (attacked) {
            TargetNextEnemy(inputDirection);
            return;
        }

        anim.applyRootMotion = false;

        if (nextEnemy) currentTarget = nextEnemy;
        else currentTarget = null;

        RaycastHit hit;
        
        if (!nextEnemy) {
            // if no input get best enemy in front or from distance
            if (inputDirection.x == 0f && inputDirection.z == 0f) {
                inputDirection = transform.forward;

                // get enemy in front
                if (Physics.SphereCast(transform.position + AdvancedWalkerController.instance.CC_Rigid.centerOfMass - new Vector3(0,2,0), accuracySize, inputDirection, out hit, detectionRadius, enemyLayers, QueryTriggerInteraction.Collide)) {
                    if (hit.transform.GetComponent<FreeflowCombatEnemy>().isAttackable) {
                        currentTarget = hit.transform.GetComponent<FreeflowCombatEnemy>();
                    }else{
                        currentTarget = null;
                    }
                }

                // if still no enemy get enemy within radius with least distance
                if (!currentTarget) {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, accuracySize, enemyLayers, QueryTriggerInteraction.Collide);
                    if (colliders.Length > 0) {
                        
                        float bestDistance = Mathf.Infinity;
                        GameObject bestEnemy = null;
                
                        foreach (var collider in colliders) {
                            if (collider.transform.GetComponent<FreeflowCombatEnemy>().isAttackable) {
                                float distance = (collider.transform.position - transform.position).sqrMagnitude;
                                if (distance < bestDistance * bestDistance) {
                                    bestDistance = distance;
                                    bestEnemy = collider.transform.gameObject;
                                }
                            }
                        }

                        currentTarget = bestEnemy.GetComponent<FreeflowCombatEnemy>();

                    }else{
                        currentTarget = null;
                    }
                }
            }else{
                if (Physics.SphereCast(transform.position + controller.CC_Rigid.centerOfMass - new Vector3(0, 2, 0), accuracySize, inputDirection, out hit, detectionRadius, enemyLayers, QueryTriggerInteraction.Collide)) {
                    if (hit.transform.GetComponent<FreeflowCombatEnemy>().isAttackable) {
                        currentTarget = hit.transform.GetComponent<FreeflowCombatEnemy>();
                    }else{
                        currentTarget = null;
                    }
                }
            }
        }

        if (!currentTarget) return;

        attacked = true;
        controller.enabled = false;
        DisableScripts();

        // set attack animation index
        if (randomizeAttackAnim) {
            attackAnimIndex = Random.Range(0, attackAnimations.Length);
        }else{
            if (attackAnimIndex+1 >= attackAnimations.Length) attackAnimIndex = 0;
            else attackAnimIndex++;
        }
        
        // set traversal animation index
        if (randomizeTraversalAnim) {
            traversalAnimIndex = Random.Range(0, traversalAnimations.Length);
        }else{
            if (traversalAnimIndex+1 >= traversalAnimations.Length) traversalAnimIndex = 0;
            else traversalAnimIndex++;
        }
        
        currentHitDist = attackAnimations[attackAnimIndex].attackDistance;
        if (useTraversalAnimations) {
            float dist = (currentTarget.transform.position - transform.position).sqrMagnitude;
            if (dist >= applyTraversalAnimDistance * applyTraversalAnimDistance) {
                animManager.Play(traversalAnimations[traversalAnimIndex].animationName, traversalAnimsTSpeed);
            }
        }
        //Vector3 VecTar;

        //transform.LookAt(currentTarget.transform);

        //VecTar = currentTarget.position;

        //transform.DOLookAt(VecTar, 0.2f);
        StartCoroutine(Lerp());
        //MoveTorwardsTarget(currentTarget, traversalTime);
        TurnTowardEnemy.instance.RoatateToEnemy(currentTarget, traversalTime);
    }


    float TargetDistance(FreeflowCombatEnemy target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, .95f);
    }

  

    void MoveTorwardsTarget(FreeflowCombatEnemy target, float duration)
    {
        //OnTrajectory.Invoke(target);
        transform.DOLookAt(target.transform.position, .2f);
        transform.DOMove(TargetOffset(target.transform), duration);
    }

    void TargetNextEnemy(Vector3 inputDirection)
    {
        RaycastHit hit;
        nextEnemy = null;
        timer = 0f;
        
        // if no input get best enemy in front or from distance
        if (inputDirection.x == 0f && inputDirection.z == 0f) {
            inputDirection = transform.forward;

            // get enemy in front
            if (Physics.SphereCast(transform.position + controller.CC_Rigid.centerOfMass, accuracySize, inputDirection, out hit, detectionRadius, enemyLayers, QueryTriggerInteraction.Collide)) {
                if (hit.transform.GetComponent<FreeflowCombatEnemy>().isAttackable) {
                    nextEnemy = hit.transform.GetComponent<FreeflowCombatEnemy>();
                }else{
                    nextEnemy = null;
                }
            }

            // if still no enemy get enemy within radius with least distance
            if (!nextEnemy) {
                Collider[] colliders = Physics.OverlapSphere(transform.position, accuracySize, enemyLayers, QueryTriggerInteraction.Collide);
                if (colliders.Length > 0) {
                    
                    float bestDistance = Mathf.Infinity;
                    GameObject bestEnemy = null;
            
                    foreach (var collider in colliders) {
                        if (collider.transform.GetComponent<FreeflowCombatEnemy>().isAttackable) {
                            float distance = (collider.transform.position - transform.position).sqrMagnitude;
                            if (distance < bestDistance * bestDistance) {
                                bestDistance = distance;
                                bestEnemy = collider.transform.gameObject;
                            }
                        }
                    }

                    nextEnemy = bestEnemy.GetComponent<FreeflowCombatEnemy>();

                }else{
                    nextEnemy = null;
                }
            }
        }else{
            if (Physics.SphereCast(transform.position + controller.CC_Rigid.centerOfMass, accuracySize, inputDirection, out hit, detectionRadius, enemyLayers, QueryTriggerInteraction.Collide)) {
                if (hit.transform.GetComponent<FreeflowCombatEnemy>().isAttackable) {
                    nextEnemy = hit.transform.GetComponent<FreeflowCombatEnemy>();
                }else{
                    nextEnemy = null;
                }
            }
        }
    }

    // lerp to attack position
    IEnumerator Lerp()
    {
        float time = 0;
        
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = currentTarget.transform.position;
        if (maintainYPosition) targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        while (time < traversalTime)
        {
            float dist = (targetPosition - transform.position).sqrMagnitude;
            bool distCheck = dist <= currentHitDist * currentHitDist;
            isTraversing = true;

            if (distCheck) {
                animManager.Play(attackAnimations[attackAnimIndex].animationName, attackAnimsTSpeed);
                isAttacking = true;
                isTraversing = false;

                yield return new WaitForSeconds(attackAnimsTSpeed);

                reachedAttackPosition = true;
                StopAllCoroutines();

                yield return null;
            }
            

            transform.position = Vector3.Lerp(startPosition, targetPosition, time / traversalTime);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    // public method for stopping the attack
    public void StopAttacking()
    {
        StopAllCoroutines();
        reachedAttackPosition = false;
        animManager.Play(idleAnimName, idleAnimTSpeed);
        isAttacking = false;
        AfterAttack();
        controller.CC_Rigid.isKinematic = false;
    }

    // cool down before re-enabling components
    void AfterAttack()
    {
        controller.enabled = true;
        EnableScripts();
        attacked = false;
        anim.applyRootMotion = defaultRootMotion;
        if (nextEnemy) Attack();
    }

    // method for reenabling the disabled scripts during attacking
    void EnableScripts()
    {
        foreach (var script in scriptsToDisable) {
            script.enabled = true;
        }
    }

    // method for disabling all set scripts in preparation for attacking
    void DisableScripts()
    {
        foreach (var script in scriptsToDisable) {
            script.enabled = false;
        }
    }
}
