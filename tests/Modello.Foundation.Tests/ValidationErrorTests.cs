namespace Modello.Foundation.Tests;

public class ValidationErrorTests
{
    [Fact]
    public void GivenValidParameters_WhenInitialize_ThenPropertiesAreSet()
    {
        // Given
        var type = "Type1";
        var error = "Error1";
        var detail = "Detail1";

        // When
        var validationError = new ValidationError(type, error, detail);

        // Then
        Assert.Equal(type, validationError.Type);
        Assert.Equal(error, validationError.Error);
        Assert.Equal(detail, validationError.Detail);
    }
}
