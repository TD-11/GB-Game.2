using UnityEngine;

public class Player : MonoBehaviour
{
   private Rigidbody2D rig;
   public float speed;
   private Vector2 movement;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Só anda na diagonal se A e D forem pressionados ao mesmo tempo
        if (Input.GetKey(KeyCode.A))
        {
            movement = new Vector2(-1, 0).normalized; // Diagonal (ajuste como quiser
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = new Vector2(1, 0).normalized; // Diagonal (ajuste como quiser)
        }
        else
        {
            movement = Vector2.zero; // Não se move
        }
    }
    
    void FixedUpdate()
    {
        rig.linearVelocity = movement * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Danger")
        {
            Destroy(gameObject);
        }
    }


}
