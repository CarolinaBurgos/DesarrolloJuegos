using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public float tickDeltaTime;
    public float fasTickDeltaTime;
    //public float speedFactor;
    private Transform trans;
    private float accDeltaTime; /*DeltaTime acumulado*/
    public TetrisBoard Board;

    /*awake() -> simula un constructor */
    void Awake()
    {
        accDeltaTime = 0f;
    }

    // Start is called before the first frame update
    /*inicializar despues del awake() */
    void Start()
    {
        trans = this.transform; /*Transformada del objeto T*/
    }

    // Update is called once per frame
    /*Se ejecutara en cada momento */
    void Update()
    {
        {/*Movimiento Y-axis */
            /*Forma 1 */
            //trans.position = new Vector3(trans.position.x, trans.position.y -1, trans.position.z); 
            
            /*Forma 2 */
            /*
            var newPos = trans.position;
            newPos.y -= 1;
            trans.position = newPos;
            */
        }

        {/*Movimiento Y-axis low*/
            accDeltaTime += Time.deltaTime;

            /*Movimiento Y-axis fast*/
            var actualMovementTick = tickDeltaTime;
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { /*Tecla shift */
                { //Move down fast
                    actualMovementTick = fasTickDeltaTime; /*Forma 1 */
                    //actualMovementTick = tickDeltaTime * speedFactor; /*Forma 2 */
                }
            }
            if(accDeltaTime>actualMovementTick){
                { //Move down
                    var oldPost = trans.position;
                    var oldRot= trans.rotation;
                    var newPost = trans.position;
                    newPost.y -= 1;
                    trans.position = newPost;

                    var (isOutX, isOutY) = ApplyConstraints(oldPost, oldRot);

                    var done = isOutY;
                    if(done) {
                        this.enabled = false;
                        Board.InstantiateTetrisBlock();
                    }                    
                }
                accDeltaTime = 0; /*Reseteo del acumulador para empezar otra vez */
            } 
        }

        {/*Movimiento X-axis */
            if(Input.GetKeyDown(KeyCode.LeftArrow)) { /*Tecla izquierda */
                { //Move left
                    var oldPost = trans.position;
                    var newPost = trans.position;
                    newPost.x -= 1;
                    trans.position = newPost;

                    /*No se salga de los bordes izquierdos*/
                    ApplyConstraints(oldPost, trans.rotation);
                }
            }

            if(Input.GetKeyDown(KeyCode.RightArrow)) { /*Tecla derecha */
                { //Move rigth
                    var oldPost = trans.position;
                    var newPost = trans.position;
                    newPost.x += 1;
                    trans.position = newPost;

                    /*No se salga de los bordes derechos*/
                    ApplyConstraints(oldPost, trans.rotation);

                }
            }

        }

        { /*Rotacion */
            if(Input.GetKeyDown(KeyCode.UpArrow)) { /*Tecla arriba */
                {
                    var oldRot= trans.rotation;
                    trans.rotation *= Quaternion.Euler(0, 0, 90); /*Forma 1 (Quaternion representa un delta de rotacion)*/
                    //trans.Rotate(new Vector3(0, 0, 90), Space.Self); /*Forma 2 (Self = coord locales)*/
                    //trans.Rotate(Vector3.forward, 90); /*Forma 3 (fordward es el eje Z)*/
                    ApplyConstraints(trans.position, oldRot);
                }
            }
        }
        
    }

    /*Verificar que el objetos no sobrepase los bordes */
    /*void bordes(Vector3 oldPost){ //Forma 1
        var isOut = false;
        foreach (var childTransform in trans.GetComponentsInChildren<Transform>()) //Devuelve transformada de los cuadrados del objeto
        {
            if (childTransform.position.x < 0 || childTransform.position.x > 10) {
                isOut = true;
                break;
            }
        }

        if(isOut){
            trans.position = oldPost;
        }
    }*/
    (bool, bool) CheckConstraints(){ //Forma 2
        var isOutX = false;
        var isOutY = false;
        for(int i = 0; i < trans.childCount; i++) {
            var childTrans = trans.GetChild(i);
            if(childTrans.position.x < 0 || childTrans.position.x > 9) {
                isOutX = true;
                break;
            }
            if(childTrans.position.y < 0) {
                isOutY = true;
                break;
            }
        }   
        return (isOutX, isOutY); 
    }
    (bool, bool) ApplyConstraints (Vector3 rollBackPos, Quaternion rollBackRot) {
        var (isOutX, isOutY) = CheckConstraints();
        if(isOutX || isOutY) {
            trans.position = rollBackPos;
            trans.rotation = rollBackRot;
        }
        return (isOutX, isOutY); 
    }
}
