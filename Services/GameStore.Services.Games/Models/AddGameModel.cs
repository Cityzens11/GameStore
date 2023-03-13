namespace GameStore.Services.Games;

using AutoMapper;
using FluentValidation;
using GameStore.Context.Entities;

public class AddGameModel
{
    public string Name { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string? ImageUri { get; set; } = string.Empty;
    public float Price { get; set; }
    public IEnumerable<string>? Genres { get; set; }
}

public class AddGameModelValidator : AbstractValidator<AddGameModel>
{
    public AddGameModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name is long.");

        RuleFor(x => x.Note)
            .NotEmpty().WithMessage("Note is required.")
            .MaximumLength(3000).WithMessage("Note is long.");

        RuleFor(x => x.Publisher)
            .NotEmpty().WithMessage("Publisher is required.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required");
    }
}

public class AddGameModelProfile : Profile
{
    public AddGameModelProfile()
    {
        CreateMap<AddGameModel, Game>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                src.Genres.Select(name => new Genre { Name = name }).ToList()));
    }
}
