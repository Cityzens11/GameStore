namespace GameStore.Api.Controllers.Models;

using AutoMapper;
using FluentValidation;
using GameStore.Services.Comments;

public class AddCommentRequest
{
    public string User { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CommentedTime { get; set; }
    public int GameId { get; set; }
}

public class AddCommentResponseValidator : AbstractValidator<AddCommentRequest>
{
    public AddCommentResponseValidator()
    {
        RuleFor(x => x.User)
            .NotEmpty().WithMessage("User is required.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body is required.")
            .MaximumLength(400).WithMessage("Body is long.");

        RuleFor(x => x.CommentedTime)
            .NotEmpty().WithMessage("CommentedTime is required.");

        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("GameId is required");
    }
}

public class AddCommentRequestProfile : Profile
{
    public AddCommentRequestProfile()
    {
        CreateMap<AddCommentRequest, AddCommentModel>()
            .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.CommentedTime));
    }
}
