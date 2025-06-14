using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TC_Intro {
    public class IntroScript : MonoBehaviour {
        void Start() {
            StartCoroutine(Wait7Seconds());
        }

        IEnumerator Wait7Seconds() {
            yield return new WaitForSeconds(7.5f);
            SceneManager.LoadScene("Main gameplay scene");
        }
    }
}
