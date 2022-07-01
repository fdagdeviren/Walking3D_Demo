using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
public class PizzaBox : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    public static bool left=false, right=false;
    private Vector3 _leftTarget;
    private Vector3 _rightTarget;
    private float _startPosYLeft;
    private float _startPosYRight;
    public int _amount;
    public static bool first = true;
    private  int _randomAmount;
    public static int _counter = 0;
    private void Start()
    {
        _counter++;
        if(_counter==10)
        {
            first = true;
            left = false;
            right = false;
            Debug.Log("Game Win");
            _counter = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        _amount = Random.Range(5, 20);
        if (!first)
            _randomAmount = Random.Range(0, 2);
        else
            _randomAmount = 0;
        if(_randomAmount==0)
        {
            first = false;
            _text.text = "+" + _amount.ToString();
            _text.color = Color.green;
        }
        else
        {
            _text.text = "-" + _amount.ToString();
            _amount = -_amount;
            _text.color = Color.red;
        }
        _leftTarget = new Vector3(-1.729f, 1.372f, 0f);
        _rightTarget = new Vector3(1.51f, 1.337f, 0f);
        _startPosYLeft = 1.372f;
    }
    void Update()
    {
        if (left == true)
        {
            var step = 90f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _leftTarget, step);
        }
        else if (right == true)
        {
            var step = 90f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _rightTarget, step);
        }
        else
        {
            transform.Translate(Vector3.back * Time.deltaTime * 10f);
        }
    }
    public void LeftOn()
    {
        left = true;
        _startPosYLeft = _startPosYLeft + 0.3f;
        _leftTarget = new Vector3(-1.729f, _startPosYLeft, 0f);
 
    }
    public void LeftOff()
    {
        left = false;
    }
    public void RightOn()
    {
        right = true;
        _startPosYRight = _startPosYRight + 0.3f;
        _leftTarget = new Vector3(-1.729f, _startPosYRight, 0f);
    }
    public void RightOff()
    {
        right = false;
    }
}
