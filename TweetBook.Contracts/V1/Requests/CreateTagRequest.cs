namespace TwitterBook.Contracts.V1.Requests;
using System.ComponentModel.DataAnnotations;

public class CreateTagRequest
{
    public string TagName { get; set; }
}