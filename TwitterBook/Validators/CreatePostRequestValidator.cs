using FluentValidation;
using Microsoft.VisualBasic;
using TwitterBook.Contracts.V1.Requests;

namespace TwitterBook.Validators;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Matches("^[a-zA-Z0-9 ]*$").Length(3, 25);
        RuleForEach(x => x.tagNames).NotNull().Length(1,10).WithMessage($"The tag should be between 1 and 10 characters")
            .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Tag name is not correct.");
    }
}