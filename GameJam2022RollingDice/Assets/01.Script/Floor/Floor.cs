using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public List<SkillSO> skillList;

    public int currentSkill;

    public int random;

    public bool isChange; //변경되야하는 바닥인가?
    public bool isFinish; //맨 마지막 도착한 바닥인가?

    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        ChangeSkill(); //랜덤스킬 및 아이콘 세팅

        StartCoroutine(MoveY()); //오브젝트 y축 떨어트리기;
    }

    public void ChangeSkill() //랜덤스킬 및 아이콘 세팅
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

        //다 떨어졌을때 착지 이펙트 추가하면 괜찮을듯;
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
        int random = Random.Range(0, System.Enum.GetValues(typeof(SkillEnum)).Length); // 0부터 스킬enum의 갯수까지 랜덤을 구함W
        return random;
    }

    public void SetMaterial(Color color)
    {
        transform.GetComponent<Renderer>().material.color = color;
    }
    
}
