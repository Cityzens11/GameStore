namespace GameStore.Services.Comments;

using AutoMapper;
using FluentValidation;
using GameStore.Context.Entities;

public class UpdateCommentModel
{
    public string User { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public int GameId { get; set; }
}

public class UpdateCommentModelValidator : AbstractValidator<UpdateCommentModel>
{
    public UpdateCommentModelValidator()
    {
        RuleFor(x => x.User)
            .NotEmpty().WithMessage("User is required.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body is required.")
            .MaximumLength(400).WithMessage("Body is long.");

        RuleFor(x => x.TimeStamp)
            .NotEmpty().WithMessage("TimeStamp is required.");

        RuleFor(x => x.GameId)
            .NotEmpty().WithMessage("GameId is required");
    }
}

public class UpdateCommentModelProfile : Profile
{
    public UpdateCommentModelProfile()
    {
        CreateMap<UpdateCommentModel, Comment>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User));
    }
}
