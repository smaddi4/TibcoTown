using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicQuestion
{
    public class QuestionFactory : MonoBehaviour
    {
        [SerializeField]
        private GameObject dropDownQuestionObj, mcqQuestionObj, textQuestionObj, phoneQuestionObj, parentObject;

        //public IQuestion GetQuestion(QuestionType type)
        //{
        //    //switch (type)
        //    //{
        //    //    case QuestionType.DROPDOWN:
        //    //        return Instantiate(dropDownQuestionObj, parentObject.transform).GetComponent<DropDownQuestion>();
        //    //    case QuestionType.MCQ:
        //    //        return Instantiate(mcqQuestionObj, parentObject.transform).GetComponent<MCQQuestion>();
        //    //    case QuestionType.TEXT:
        //    //        return Instantiate(textQuestionObj, parentObject.transform).GetComponent<TextQuestion>();
        //    //    case QuestionType.PHONE:
        //    //        return Instantiate(phoneQuestionObj, parentObject.transform).GetComponent<PhoneQuestion>();
        //    //    default:
        //    //        return null;
        //    //}
        //}
    } 
}
