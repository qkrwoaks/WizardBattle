using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Canvas컴포넌트를 사용해야 하므로 추가.
using UnityEngine.EventSystems;    //PointerEventData를 사용해야 하므로 추가.

public class OnMouseCheckController : MonoBehaviour
{
    [Header("Check")]
    [SerializeField] Canvas m_canvas;           //사용하는 Canvas 넣기
    GraphicRaycaster m_gr;
    PointerEventData m_ped;

    [SerializeField] GameObject descriptionImagePrefab;
    [SerializeField] private bool isShow;

    private void Awake()
    {
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);
    }

    void Update()
    {
        OnMouseCheck();
    }

    /// <summary>
    /// 마우스가 이미지 위에 있을 때 체크
    /// </summary>
    private void OnMouseCheck()
    {
        m_ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_gr.Raycast(m_ped, results);

        if (results.Count > 0 && !isShow)
        {
            if (results[0].gameObject.TryGetComponent<SkillChooseButton>(out SkillChooseButton button))
            {
                StartCoroutine(ShowDescriptionImageCoroutine(button));
            }
        }
    }

    IEnumerator ShowDescriptionImageCoroutine(SkillChooseButton button)
    {
        isShow = true;
        GameObject descriptionObj = null;
        while (true)
        {
            //설명 이미지 생성
            if (descriptionObj == null)
            {
                descriptionObj = Instantiate(descriptionImagePrefab, button.transform.position + new Vector3(-200f,-100f,0), Quaternion.identity);
                descriptionObj.transform.SetParent(m_canvas.transform);
                descriptionObj.GetComponent<RectTransform>().localScale = Vector3.one;

                InitObj(descriptionObj, button);
            }

            #region Check OnMouse

            m_ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_gr.Raycast(m_ped, results);

            if (!results[0].gameObject.transform.GetComponent<SkillChooseButton>())
            {
                isShow = false;
                Destroy(descriptionObj);
                yield break;
            }

            #endregion
            yield return new WaitForEndOfFrame();
        }
    }

    private void InitObj(GameObject deObj, SkillChooseButton btn)
    {
        DescriptionImage deImg = deObj.GetComponent<DescriptionImage>();

        deImg.nameText.text = btn.skillData.SkillName;
        deImg.descriptionText.text = btn.skillData.Description;      //text에 설명 받아오기
    }
}
