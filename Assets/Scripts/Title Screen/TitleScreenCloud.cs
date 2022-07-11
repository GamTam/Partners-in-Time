using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TitleScreenCloud : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private double _speed;
    [SerializeField] private double _minSpeed = 2;
    [SerializeField] private double _maxSpeed = 5;
    
    private System.Random _rand = new System.Random();
    
    // Start is called before the first frame update
    void Start()
    {
        _speed = (_rand.NextDouble() * (_maxSpeed - _minSpeed) + _minSpeed);
        double scale = (_rand.NextDouble() * (1.25 - 0.5) + 0.5);
        double yModifier = _rand.NextDouble() - 0.5;
        double zModifier = _rand.NextDouble() - 0.5;
        transform.localScale = new Vector3((float) scale, (float) scale, (float) scale);
        transform.position = new Vector3(transform.position.x, (float) (transform.position.y + yModifier),
            (float) (transform.position.z + zModifier));
        // GetComponent<SpriteRenderer>().sprite = _sprites[_rand.Next(_sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3((float) (transform.position.x - _speed * Time.deltaTime), transform.position.y, transform.position.z);
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }
}
