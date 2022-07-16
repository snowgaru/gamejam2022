using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public List<SkillSO> skillList;

    public int currentSkill;

    public int random;

    public bool isChange; //����Ǿ��ϴ� �ٴ��ΰ�?
    public bool isFinish; //�� ������ ������ �ٴ��ΰ�?

    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        ChangeSkill(); //������ų �� ������ ����

        StartCoroutine(MoveY()); //������Ʈ y�� ����Ʈ����;
    }

    public void ChangeSkill() //������ų �� ������ ����
    {

        GetRandom();
        SetIcon();
        SetSkill();

    }

    private void GetRandom()
    {
        random = GetRandomSkill();
    }

    private IEnumerator MoveY()
    {
        for(int i = 1; i <= 20; i++)
        {
            transform.position = new Vector3(transform.position.x, 5 - (0.2f * i), transform.position.z);
            yield return new WaitForSeconds(0.005f);
        }

        //�� ���������� ���� ����Ʈ �߰��ϸ� ��������;
    }

    private void SetSkill()
    {
        currentSkill = random;
    }

    public void SetIcon()
    {
        spriteRenderer.sprite = skillList[random].icon;
    }

    public int GetRandomSkill()
    {
        int random = Random.Range(0, System.Enum.GetValues(typeof(SkillEnum)).Length); // 0���� ��ųenum�� �������� ������ ����W
        return random;
    }

    public void SetMaterial(Color color)
    {
        transform.GetComponent<Renderer>().material.color = color;
    }
    
}
