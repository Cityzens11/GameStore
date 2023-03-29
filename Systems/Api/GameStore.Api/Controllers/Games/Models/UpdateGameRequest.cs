namespace GameStore.Api.Controllers.Models;

using AutoMapper;
using GameStore.Services.Games;
using FluentValidation;

public class UpdateGameRequest
{
    public int? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string? ImageUri { get; set; } = string.Empty;
    public float Price { get; set; }
    public IEnumerable<string>? Genres { get; set; }
}

public class UpdateGameRequestValidator : AbstractValidator<UpdateGameRequest>
{
    public UpdateGameRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title is long.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(3000).WithMessage("Description is long.");

        RuleFor(x => x.Publisher)
            .NotEmpty().WithMessage("Publisher is required.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required");
    }
}

public class UpdateBookRequestProfile : Profile
{
    public UpdateBookRequestProfile()
    {
        CreateMap<UpdateGameRequest, UpdateGameModel>()
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));

    }
}
