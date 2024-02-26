﻿using Fintracker.Application.DTO.Category;
using Fintracker.Application.DTO.Common;
using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.DTO.Budget;

public class BudgetBaseDTO : IBaseDto
{
    public Guid Id { get; set; }

    public ICollection<CategoryDTO> Categories { get; set; }
    
    public string Name { get; set; }

    public decimal Balance { get; set; }

    public CurrencyDTO Currency { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public Guid UserId { get; set; }
}