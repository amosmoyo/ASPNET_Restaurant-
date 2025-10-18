using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.Commands
{
    public class CreateDishValidators: AbstractValidator<CreateDishCommand>
    {
        public CreateDishValidators() 
        {
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required")
                .Must(p => double.TryParse(p, out var value) && value > 0).WithMessage("Price must be a greate than zero");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name of the dish ir required")
                .MinimumLength(10).WithMessage("Length must be greater than 10");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description of the dish is required");
                  
        }
    }
}
