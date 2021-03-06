﻿using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Core
{
    public class Asteroid : AsteroidGameObject, IKillable
    {
        public GameObject SmallerAsteroidPrefab = null;
        public float MaxMoveSpeed;

        void Start()
        {
            Assert.IsTrue(MaxMoveSpeed != 0.0f);

            SetRandomInitialMoveAngle();
        }

        private void SetRandomInitialMoveAngle()
        {
            var randomAngle = Random.Range(0, 180);
            var rotationVector = new Vector3(0, 0, randomAngle);
            var direction = Quaternion.Euler(rotationVector).eulerAngles;
            transform.Rotate(direction);
        }

        private void SetInitialVelocity()
        {
            var rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.velocity = transform.up * MaxMoveSpeed;
        }

        void Update()
        {
            SetInitialVelocity();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Die();
        }

        public void Die()
        {
            TrySplitToTwoSmallerAsteroids();
            Destroy(gameObject);
        }

        private void TrySplitToTwoSmallerAsteroids()
        {
            if (!CanSplitToSmallerAsteroids())
            {
                return;
            }

            CreateAsteroidRotatedBy(90);
            CreateAsteroidRotatedBy(-90);
        }

        private bool CanSplitToSmallerAsteroids()
        {
            return SmallerAsteroidPrefab != null;
        }

        private GameObject CreateAsteroidRotatedBy(float angle)
        {
            var newAsteroid = Instantiate(SmallerAsteroidPrefab, transform.position, Quaternion.identity);
            var z = transform.rotation.eulerAngles.z + angle;
            newAsteroid.transform.rotation = Quaternion.Euler(0, 0, z);
            return newAsteroid;
        }
    }
}
