namespace LightingScada.Models;

/// <summary>Обліковий запис оператора з розділу Auth:Users файлу appsettings.json.</summary>
public class UserCredential
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}
