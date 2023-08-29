using FluentValidation;
using TwitterBook.Contracts.V1.Requests;

namespace TwitterBook.Validators;

public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagRequestValidator()
    {
        RuleFor(x=> x.TagName).NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*$").Length(3,15);
    }
}