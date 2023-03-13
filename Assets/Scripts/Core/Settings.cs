using System;
using UnityEngine;

namespace Core
{
    public class Settings : MonoBehaviour
    {
        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }
    }
}