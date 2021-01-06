using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurveySystem.WebApi
{
    public enum AnswerEventTypeEnum
    {
        [Display(Name = "Нажал кнопку мыши")]
        [Description("Нажал кнопку мыши")]
        Click = 0,
        [Display(Name = "Сдвинул мышь")]
        [Description("Сдвинул мышь")]
        Move = 1,
        [Display(Name = "Перетащил содержимое")]
        [Description("Перетащил содержимое")]
        Drag = 2,
        [Display(Name = "Нажал клавишу")]
        [Description("Нажал клавишу")]
        Press = 3,
        [Display(Name = "Другое действие")]
        [Description("Другое действие")]
        Other = 4
    }
}
