namespace GameStore.Api.Controllers.Models;

using AutoMapper;
using FluentValidation;
using GameStore.Services.Games;

public class AddGameRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string? ImageUri { get; set; } = string.Empty;
    public float Price { get; set; }
    public IEnumerable<string>? Genres { get; set; }
}

public class AddGameResponseValidator : AbstractValidator<AddGameRequest>
{
    public AddGameResponseValidator()
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

public class AddGameRequestProfile : Profile
{
    public AddGameRequestProfile()
    {
        CreateMap<AddGameRequest, AddGameModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));
    }
}

