#region Using Statements
using UnityEngine;
using UnityEngine.UI;
#endregion

public class UnitUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Text lblHealth;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject tick;
    [SerializeField] Transform start;

    [Header("Armour")]
    [SerializeField] GameObject sheild;
    [SerializeField] Text lblArmour;
    Camera cam;

    int maxHealth = 0;
    #region MonoBehaviour Callbacks
    void Start()
    {
        cam = Camera.main;
    }

    public void FixedUpdate()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

    }
    #endregion

    #region Private Methods
    private void UpdateTicks()
    {
        float healthBarWidth = healthBar.GetComponent<RectTransform>().sizeDelta.x;
        float increment = healthBarWidth / maxHealth;
        float position = 0;
        Debug.Log(start.localPosition);
        for (int i = 1; i < maxHealth; i++)
        {
            position -= increment;
            GameObject newTick = Instantiate(tick);
            newTick.transform.SetParent(start, false);
            newTick.transform.localPosition = new Vector3(position, 0, 0);
            //if (i % 5 == 0)
            //{
            //    newTick.transform.localScale += new Vector3(1, 0, 0);
            //}
        }
    }
    #endregion

    #region Public Methods
    public void UpdateHealth(int health)
    {
        lblHealth.text = "Health: " + health;
        
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void UpdateHealth(int health, int newMax)
    {
        maxHealth = newMax;
        lblHealth.text = "Health: " + health;
        healthBar.fillAmount = health / maxHealth;
        UpdateTicks();
    }

    public void UpdateArmour(int armour)
    {
        Debug.Log("update armour " + armour);
        lblArmour.text = armour.ToString();
        if(armour > 0)
        {
            sheild.gameObject.SetActive(true);
        }
        else
        {
            sheild.gameObject.SetActive(false);
        }

    }

    #endregion
}
