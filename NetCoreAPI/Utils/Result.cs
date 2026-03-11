namespace NetCoreAPI.Utils;

/// <summary>
/// Classe générique pour représenter le résultat d'une opération qui peut réussir ou échouer.
/// Permet d'éviter l'utilisation d'exceptions pour la logique métier normale.
/// </summary>
/// <typeparam name="T">Type de la valeur retournée en cas de succès.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Indique si l'opération a réussi.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Valeur retournée en cas de succès.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Message d'erreur en cas d'échec.
    /// </summary>
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Crée un résultat de succès avec une valeur.
    /// </summary>
    /// <param name="value">Valeur à retourner.</param>
    /// <returns>Instance de Result représentant un succès.</returns>
    public static Result<T> Success(T value) => new(true, value, null);

    /// <summary>
    /// Crée un résultat d'échec avec un message d'erreur.
    /// </summary>
    /// <param name="error">Message d'erreur.</param>
    /// <returns>Instance de Result représentant un échec.</returns>
    public static Result<T> Failure(string error) => new(false, default, error);
}
