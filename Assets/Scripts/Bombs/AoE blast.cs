using UnityEngine;

public class AoEblast : MonoBehaviour
{
    [SerializeField] private Ball ball;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ball.ExplosiveHit(other.gameObject);
            Debug.Log("player blasted");
        }
    }
}
