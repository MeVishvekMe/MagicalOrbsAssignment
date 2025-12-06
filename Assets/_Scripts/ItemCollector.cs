using System;
using System.Collections;
using UnityEngine;

public class ItemCollector : MonoBehaviour {
    [SerializeField] private GameObject highlightGameObject;
    
    private Transform target;
    
    private float _heightOffset = 3.5f;
    private float _moveSpeed = 10f;
    private float _rotateSmooth = 10f;

    private bool isCollected = false;
    
    private void OnTriggerEnter(Collider other) {
        if (isCollected) return;
        
        if (other.CompareTag("Player")) {
            target = other.transform.Find("Visual/Backpack");
            if (target != null) {
                highlightGameObject.SetActive(false);
                isCollected = true;
                GetComponent<Collider>().enabled = false;  // prevent retrigger

                StartCoroutine(FlyToBackpack(target));
            }
        }
    }

    IEnumerator FlyToBackpack(Transform backpack) {
        // ---------------------------------------------------
        // 1) Fly to ABOVE the backpack
        // ---------------------------------------------------
        Vector3 targetAbove = backpack.position + Vector3.up * _heightOffset;

        while (Vector3.Distance(transform.position, targetAbove) > 0.15f) {
            // face direction
            Vector3 dir = (targetAbove - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * _rotateSmooth);

            // move
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetAbove,
                _moveSpeed * Time.deltaTime
            );

            yield return null; // wait 1 frame
        }

        // ---------------------------------------------------
        // 2) Drop INTO the backpack
        // ---------------------------------------------------
        while (Vector3.Distance(transform.position, backpack.position) > 0.1f) {
            Vector3 dir = (backpack.position - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * _rotateSmooth);

            transform.position = Vector3.MoveTowards(
                transform.position,
                backpack.position,
                _moveSpeed * Time.deltaTime * 1.5f // a bit faster
            );

            yield return null;
        }
    }

}
