namespace GameStore.Services.Games;

using AutoMapper;
using FluentValidation;
using GameStore.Context.Entities;

public class UpdateGameModel
{
    public string Name { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string? ImageUri { get; set; } = string.Empty;
    public float Price { get; set; }
    public IEnumerable<string>? Genres { get; set; }
}

public class UpdateGameModelValidator : AbstractValidator<UpdateGameModel>
{
    public UpdateGameModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(25).WithMessage("Name is long.");

        RuleFor(x => x.Note)
            .NotEmpty().WithMessage("Note is required.")
            .MaximumLength(2000).WithMessage("Note is long.");

        RuleFor(x => x.Publisher)
            .NotEmpty().WithMessage("Publisher is required.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required");
    }
}

public class UpdateGameModelProfile : Profile
{
    public UpdateGameModelProfile()
    {
        CreateMap<UpdateGameModel, Game>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                src.Genres.Select(name => new Genre { Name = name }).ToList()));
    }
}
