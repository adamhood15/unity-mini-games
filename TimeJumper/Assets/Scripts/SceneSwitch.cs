using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
   void OntriggerEnter(Collider other)
   {
    SceneManager.LoadScene(1);
   }
}
