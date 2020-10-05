using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBehaviour : MonoBehaviour
{
    private Animation _animation;
    public int randomTimeMax;
    public int randomTimeMin;
    public List<String> animationNames;
    
    public bool isWalking = false;
    public bool shoudRotate = false;
    public Rigidbody rb;
    public float velocity = 1;
    public float angularVelocity = 200;
        void Start()
    {
        _animation = this.GetComponent<Animation>();
        rb = this.GetComponent<Rigidbody>();
        StartCoroutine(waiter());

        
    }

        private void Update()
        {
            if (isWalking)
            {
                if (shoudRotate)
                {
                    transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * angularVelocity, Space.World);
                }
                rb.velocity = transform.forward * velocity * Time.deltaTime;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }

        IEnumerator waiter()
    {
        int wait_time = Random.Range (randomTimeMin, randomTimeMax);
        yield return new WaitForSeconds (wait_time);
        String nextAnimation = animationNames[Random.Range(0, animationNames.Count)];
        isWalking = nextAnimation.Equals("Walk");
        _animation.CrossFade(nextAnimation);
        //_animation.Play(nextAnimation, AnimationPlayMode.Mix);
        StartCoroutine(waiter());
    }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag != "Floor")
            {
                shoudRotate = true;
            }

        }

        private void OnCollisionExit(Collision other)
        {
            
            if (other.gameObject.tag != "Floor")
            {
                shoudRotate = false;
            }
        }
}
