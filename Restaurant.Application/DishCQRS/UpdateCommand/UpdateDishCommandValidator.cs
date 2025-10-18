using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DishCQRS.UpdateCommand
{
    public class UpdateDishCommandValidator: AbstractValidator<UpdateDishCommandV2>
    {
        public UpdateDishCommandValidator() 
        {
            RuleFor(x => x.Price)
               .NotEmpty().WithMessage("Price is required")
               .Must(p => double.TryParse(p, out var price) && price > 0).WithMessage("Price must be greater than zero");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Dish id is required")
                .GreaterThan(0).WithMessage("Dish id must be greater than zero");
        }
    }
}
