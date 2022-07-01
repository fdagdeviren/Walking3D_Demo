using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _clone;
    [SerializeField] private GameObject _pizzaClone;
    [SerializeField] private GameObject _ragdollObject;
    [SerializeField] private GameObject Cylinder;
    [SerializeField] private GameObject _leftTransform;
    [SerializeField] private GameObject _rightTransform;
    [SerializeField] private GameObject _retryButton;
    public static bool left = false, right = false;
    private float _startPosYLeft;
    private float _startPosYRight;

    private Vector3 currentAngle;
    private List<GameObject> PizzaBoxsLeft = new List<GameObject>();
    private List<GameObject> PizzaBoxsRight = new List<GameObject>();
    private List<GameObject> PizzaBoxWaysLeft = new List<GameObject>();
    private List<GameObject> PizzaBoxWaysRight = new List<GameObject>();
    private GameObject _fakeRagdollObject;

    private void OnMouseDrag()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("hey");
        }
    }
    private void Start()
    {
        OnPizzaBoxCreate();
        currentAngle = transform.eulerAngles;
        _startPosYLeft = 1.174f;
        _startPosYRight = 1.174f;
    }
    private void Update()
    {
        if(PizzaBoxsLeft.Count-PizzaBoxsRight.Count>5)
        {
            _animator.SetTrigger("LeftFail1");
            currentAngle = new Vector3(
             Mathf.LerpAngle(currentAngle.x, 0, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.y, 0, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.z, 30, Time.deltaTime));

            transform.eulerAngles = currentAngle;
        }
        else if (PizzaBoxsRight.Count-PizzaBoxsLeft.Count>5)
        {
            _animator.SetTrigger("RightFail1");
            currentAngle = new Vector3(
             Mathf.LerpAngle(currentAngle.x, 0, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.y, 0, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.z, -30, Time.deltaTime));

            transform.eulerAngles = currentAngle;
        }
        else if (PizzaBoxsRight.Count == PizzaBoxsLeft.Count)
        {
             _animator.SetTrigger("idle");
            currentAngle = new Vector3(
             Mathf.LerpAngle(currentAngle.x, 0, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.y, 0, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.z, 0, Time.deltaTime));
        }
        if (PizzaBoxsLeft.Count>0)
        {
            for (int i = 1; i < PizzaBoxsLeft.Count; i++)
                PizzaBoxsLeft[i].transform.position = new Vector3(PizzaBoxsLeft[i - 1].transform.position.x, PizzaBoxsLeft[i - 1].transform.position.y + 0.1f, PizzaBoxsLeft[i - 1].transform.position.z);
        }
        if (PizzaBoxsRight.Count > 0)
        {
            for (int i = 1; i < PizzaBoxsRight.Count; i++)
                PizzaBoxsRight[i].transform.position = new Vector3(PizzaBoxsRight[i - 1].transform.position.x, PizzaBoxsRight[i - 1].transform.position.y + 0.1f, PizzaBoxsRight[i - 1].transform.position.z);
        }
    }
    public void LeftSwipe()
    {
        PizzaBoxWaysLeft.Add(PizzaBoxClone);
        if (PizzaBoxWaysLeft[0].GetComponent<PizzaBox>()._amount<0)
        {
            for (int i = 0; i < -PizzaBoxWaysLeft[0].GetComponent<PizzaBox>()._amount; i++)
                OnCloneDestroyLeft();
            PizzaBoxWaysLeft[0].GetComponent<PizzaBox>().LeftOn();
        }
        else
        {
            PizzaBoxWaysLeft[0].GetComponent<PizzaBox>().LeftOn();
            LeftOn();
        }
    }
    public void RightSwipe()
    {
        PizzaBoxWaysRight.Add(PizzaBoxClone);
        if (PizzaBoxWaysRight[0].GetComponent<PizzaBox>()._amount < 0)
        {
            PizzaBoxWaysRight[0].GetComponent<PizzaBox>().RightOn();
            for (int i = 0; i < -PizzaBoxWaysRight[0].GetComponent<PizzaBox>()._amount; i++)
                OnCloneDestroyRight();
        }
        else
        {
            PizzaBoxWaysRight[0].GetComponent<PizzaBox>().RightOn();
            RightOn();
        }
    }
    public void RightOn()
    {
        right = true;
        for (int i = 0; i < PizzaBoxWaysRight[0].GetComponent<PizzaBox>()._amount; i++)
            OnCloneCreateRight();
        BalanceControl();
        StartCoroutine(coroutineRight());
    }
    public void LeftOn()
    {
        left = true;
        for (int i = 0; i < PizzaBoxWaysLeft[0].GetComponent<PizzaBox>()._amount; i++)
            OnCloneCreateLeft();
        BalanceControl();
        StartCoroutine(coroutineLeft());
    }

    public void BalanceControl()
    {
        if (PizzaBoxsRight.Count-PizzaBoxsLeft.Count>32)
        {
            Debug.Log("Game Lose");
            _ragdollObject.SetActive(true);
            _retryButton.SetActive(true);
            _ragdollObject.GetComponent<Animator>().SetTrigger("Right");
            for (int i = 0; i < PizzaBoxsRight.Count; i++)
            {
                PizzaBoxsRight[i].GetComponent<Rigidbody>().useGravity = true;
            }
            for (int i = 0; i < PizzaBoxsLeft.Count; i++)
            {
                PizzaBoxsLeft[i].GetComponent<Rigidbody>().useGravity = true;
            }
            Cylinder.GetComponent<Rigidbody>().useGravity = true;
        }
        if (PizzaBoxsLeft.Count - PizzaBoxsRight.Count > 32)
        {
            Debug.Log("Game Lose");
            _retryButton.SetActive(true);
            _ragdollObject.SetActive(true);
            _ragdollObject.GetComponent<Animator>().SetTrigger("Left");
            for (int i = 0; i < PizzaBoxsLeft.Count; i++)
            {
                PizzaBoxsLeft[i].GetComponent<Rigidbody>().useGravity = true;
            }
            for (int i = 0; i < PizzaBoxsRight.Count; i++)
            {
                PizzaBoxsRight[i].GetComponent<Rigidbody>().useGravity = true;
            }
            Cylinder.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void OnCloneCreateLeft()
    {
        _startPosYLeft = _startPosYLeft+0.1f;
        _leftTransform.transform.position = new Vector3(_leftTransform.transform.position.x, _startPosYLeft, _leftTransform.transform.position.z);
        GameObject Clone=Instantiate(_clone, _leftTransform.transform.position, _leftTransform.transform.rotation);
        PizzaBoxsLeft.Add(Clone);
        Clone.transform.parent = transform;
    }
    public void OnCloneCreateRight()
    {
        _startPosYRight = _startPosYRight + 0.1f;
        _rightTransform.transform.position = new Vector3(_rightTransform.transform.position.x, _startPosYRight, _rightTransform.transform.position.z);
        GameObject Clone = Instantiate(_clone, _rightTransform.transform.position, _rightTransform.transform.rotation);
        PizzaBoxsRight.Add(Clone);
        Clone.transform.parent = transform;
    }
    GameObject PizzaBoxClone;
    public void OnPizzaBoxCreate()
    {
        PizzaBoxClone = Instantiate(_pizzaClone, new Vector3(0.089f, 0, 50f), Quaternion.identity);
    }
    public void OnCloneDestroyLeft()
    {
        if (PizzaBoxsLeft.Count > 0)
        {
            _startPosYLeft = _startPosYLeft - 0.1f;
            Destroy(PizzaBoxsLeft[PizzaBoxsLeft.Count - 1].gameObject);
            PizzaBoxsLeft.Remove(PizzaBoxsLeft[PizzaBoxsLeft.Count - 1]);
            StartCoroutine(coroutineLeft());
        }
    }
    public void OnCloneDestroyRight()
    {
        if (PizzaBoxsRight.Count > 0)
        {
            _startPosYRight = _startPosYRight - 0.1f;
            Destroy(PizzaBoxsRight[PizzaBoxsRight.Count - 1].gameObject);
            PizzaBoxsRight.Remove(PizzaBoxsRight[PizzaBoxsRight.Count - 1]);
            StartCoroutine(coroutineRight());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="PizzaBox")
        {
            other.gameObject.SetActive(false);
            Debug.Log("Game Lose");
            _ragdollObject.SetActive(true);
            _ragdollObject.GetComponent<Animator>().SetTrigger("Normal");
            _retryButton.SetActive(true);
        }
        if(other.tag=="Ragdoll")
        {
            _fakeRagdollObject = other.gameObject;
            StartCoroutine(coroutineA());
        }
    }
    IEnumerator coroutineA()
    {
        yield return new WaitForSeconds(0.1f);
        _fakeRagdollObject.gameObject.SetActive(false);
    }
    IEnumerator coroutineLeft()
    {
        yield return new WaitForSeconds(0.45f);
        try
        {
            PizzaBoxWaysLeft[0].GetComponent<PizzaBox>().LeftOff();
            Destroy(PizzaBoxWaysLeft[0]);
            PizzaBoxWaysLeft.Remove(PizzaBoxWaysLeft[0]);
            OnPizzaBoxCreate();
        }
        catch   { }
    }
    IEnumerator coroutineRight()
    {
        yield return new WaitForSeconds(0.45f);
        try
        {
         PizzaBoxWaysRight[0].GetComponent<PizzaBox>().RightOff();
        Destroy(PizzaBoxWaysRight[0]);
        PizzaBoxWaysRight.Remove(PizzaBoxWaysRight[0]);
        OnPizzaBoxCreate();
        }
        catch { }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
