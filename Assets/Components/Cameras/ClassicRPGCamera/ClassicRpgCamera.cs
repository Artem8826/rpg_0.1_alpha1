﻿using System.Collections;
using UnityEngine;

namespace Assets.Components.Cameras.ClassicRPGCamera
{
    public class ClassicRpgCamera : MonoBehaviour
    {
        public GameObject SpawnMarker;

        public Transform target;
        public float targetHeight = 1.2f;
        public float targetSide = -0.15f;
        public float distance = 4.0f;
        public float maxDistance = 6;
        public float minDistance = 1.0f;

        [Header("Horizontal Axis")]
        public float xSpeed = 250.0f;
        public float StartXRotation = 0;
        public bool AllowXRotation = true;

        [Header("Vertical Axis")]
        public float ySpeed = 120.0f;
        public float yMinLimit = -10;
        public float yMaxLimit = 70;
        public float StartYRotation = 45;
        public bool AllowYRotation = true;

        [Header("Next Divide Axis")]
        public float zoomRate = 80;
        public float rotationDampening = 3.0f;
        public Quaternion aim;
        public float aimAngle = 8;

        RaycastHit hit;

        [HideInInspector]
        public float shakeValue = 0.0f;
        [HideInInspector]
        public bool onShaking = false;
        private float shakingv = 0.0f;
        public bool lockOn = false;
	
        public JoystickCanvas aimStick; //For Mobile
        public bool mobileMode = false;

        public bool FreeFly = false;

        private float x = 20.0f;
        private float y = 0.0f;

        void Awake(){
            DontDestroyOnLoad(transform.gameObject);
            if(!target){
                target = GameObject.FindWithTag("Player").transform;
            }
            Vector3 angles = transform.eulerAngles;
            x = StartXRotation;
            y = StartYRotation;

            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;
            //Screen.lockCursor = true;
//            Cursor.lockState = CursorLockMode.Locked;
//            Cursor.visible = false;
        }
        protected Vector3 GetToRotation()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
            {
                return raycastHit.point;
            }

            return Vector3.zero;
        }

        void LateUpdate()
        {
            y = AllowYRotation ? ClampAngle(y, yMinLimit, yMaxLimit) : StartYRotation;
            x = AllowXRotation ? x : StartXRotation;

            if (onShaking && GlobalConditionC.freezeCam){
                shakeValue = Random.Range(-shakingv , shakingv)* 0.2f;
                transform.position += new Vector3(0,shakeValue,0);
            }
            if(!target || GlobalConditionC.freezeCam){
                return;
            }
		
            if(Time.timeScale == 0.0f){
                return;
            }
		
            /*if(!mobileMode){
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
		
            if(aimStick){
                float aimHorizontal = aimStick.position.x;
                float aimVertical = aimStick.position.y;
                if(aimHorizontal != 0 || aimVertical != 0){
                    x += aimHorizontal * xSpeed * 0.02f;
                    y -= aimVertical * ySpeed * 0.02f;
                }
            }*/
		
            distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
		
            // Rotate Camera
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            transform.rotation = rotation;
            aim = Quaternion.Euler(y- aimAngle, x, 0);
		
            //Rotate Target
            //if(Input.GetButton("Fire1") || Input.GetButton("Fire2") || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || target.GetComponent<AttackTrigger>() && target.GetComponent<AttackTrigger>().onAttacking || lockOn){
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || target.GetComponent<AttackTriggerC>() && target.GetComponent<AttackTriggerC>().isCasting || lockOn){
                if(target.GetComponent<StatusC>() && !target.GetComponent<StatusC>().freeze && !GlobalConditionC.freezeAll && !GlobalConditionC.freezePlayer){
                    if (!FreeFly)
                    {
                        target.transform.rotation = Quaternion.Euler(0, x, 0);
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
//                Debug.Log(GetToRotation());
//                Instantiate(SpawnMarker, GetToRotation(), Quaternion.identity);
                Vector3 eulerLook = Vector3.zero;
//                eulerLook.y = Quaternion.LookRotation(target.transform.position - GetToRotation()).eulerAngles.y;
                eulerLook.y = Quaternion.LookRotation(GetToRotation() - target.transform.position).eulerAngles.y;

                target.transform.rotation = Quaternion.Euler(eulerLook);
//                target.Rotate(Vector3.up * Time.deltaTime * 10);
            }
		
            //Camera Position
            Vector3 position = target.position - (rotation * new Vector3(targetSide , 0 , 1) * distance + new Vector3(0,-targetHeight,0));
            transform.position = position;
		
            RaycastHit hit;
            Vector3 trueTargetPosition = target.position - new Vector3(targetSide,-targetHeight,0);
		
            if(Physics.Linecast (trueTargetPosition, transform.position, out hit)){
                if(hit.transform.tag == "Wall"){
                    transform.position = hit.point + hit.normal*0.1f;   //put it at the position that it hit
                }
            }
            if(onShaking){
                shakeValue = Random.Range(-shakingv , shakingv)* 0.2f;
                transform.position += new Vector3(0,shakeValue,0);
            }
        }
	
        static float ClampAngle(float angle , float min , float max){
            if(angle < -360)
                angle += 360;
            if(angle > 360)
                angle -= 360;
            return Mathf.Clamp (angle, min, max);
		
        }
	
        public void Shake(float val , float dur){
            if(onShaking){
                return;
            }
            shakingv = val;
            StartCoroutine(Shaking(dur));
        }
	
        public IEnumerator Shaking(float dur){
            onShaking = true;
            yield return new WaitForSeconds(dur);
            shakingv = 0;
            shakeValue = 0;
            onShaking = false;
        }
	
        public void SetNewTarget(Transform p){
            target = p;
        }
	
        void OnEnable(){
            shakingv = 0;
            shakeValue = 0;
            onShaking = false;
        }
    }
}