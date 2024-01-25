using System;
using UnityEngine;

namespace _Project.Scirpts.Game.Platforms
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public abstract class Platform : MonoBehaviour
    {
        protected abstract void OnTriggerEnter(Collider other);
    }
}