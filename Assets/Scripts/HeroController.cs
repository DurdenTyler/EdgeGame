using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class HeroController: MonoBehaviour
    {
        [SerializeField] private float _rollSpeed = 5f;
        private Vector3 _pivotPoint;
        private Vector3 _axis;
        private bool _isMoving;
        private Rigidbody _rigidBody;


        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_isMoving) return;
            
            // Возвращает тру на каждом кадре если кнопка нажата
            if (Input.GetKey(KeyCode.A))
            {
                Move(Vector3.left);
            } else if (Input.GetKey(KeyCode.D))
            {
                Move(Vector3.right);
            } else if (Input.GetKey(KeyCode.W))
            {
                Move(Vector3.forward);
            } else if (Input.GetKey(KeyCode.S))
            {
                Move(Vector3.back);
            }
        }
        
        private bool HasWallInDirection(Vector3 direction)
        {
            return Physics.Raycast(transform.position, direction, 0.55f);
        }

        private void Move(Vector3 direction)
        {
            var hasWall = HasWallInDirection(direction);
            var verticalComponent = Vector3.down;
            if (hasWall)
            {
                verticalComponent = Vector3.up;
            }
            
            // получили точку в зависимости от направления куда двигаемся
            _pivotPoint = (direction / 2f) + (verticalComponent / 2f) + transform.position;
            
            // получили ось(получили перпендекуляр 2ух векторов(это нам надо для поворота))
            _axis = Vector3.Cross(Vector3.up, direction);
            
            
            // корутина - эта вещь очень близкая к многопоточности, а многопоточность это возможность запускать
            // несколько потоков выполнения программ паралелльнно, ТО ЕСТЬ ПРОСТО ПАРАЛЕЛЛЬНО ЧТО ТО ВЫПОЛНЯЕМ
            StartCoroutine(Roll(_pivotPoint, _axis));
        }

        private IEnumerator Roll(Vector3 pivot, Vector3 axis)
        {
            _isMoving = true;
            _rigidBody.isKinematic = true;

            for (int i = 0; i < 90 / _rollSpeed; i++)
            {
                transform.RotateAround(pivot, axis, _rollSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            // Обычная программа прошла бы этот цикл очень быстро и никакой анимации бы не произошло,
            // но корутина позволяет нам подождать некоторое время
            // перед выполнением очередного цикла


            _rigidBody.isKinematic = false;
            _isMoving = false;
        }











        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawSphere(_pivotPoint, 0.2f);
            //Gizmos.DrawRay(_pivotPoint, _axis);
        }
    }
} 