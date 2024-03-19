namespace MilkrunApi.Exceptions;

public class ProductExistsException(string message) : Exception(message);