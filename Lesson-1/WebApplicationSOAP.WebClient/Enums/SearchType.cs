using System.ComponentModel.DataAnnotations;

public enum SearchType
{
    [Display(Name ="Заголовок")]
    Title,
    [Display(Name = "Автор")]
    Author,
    [Display(Name = "Категоря")]
    Category
}
