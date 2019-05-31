using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlatform : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<MeepController>().canPlatform = true;
    }
}
