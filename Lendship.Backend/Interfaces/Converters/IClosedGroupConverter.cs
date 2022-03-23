﻿using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IClosedGroupConverter
    {
        ClosedGroupDto ConvertToDto(ClosedGroup closedGroup, List<ApplicationUser> users);

        ClosedGroup ConvertToEntity(ClosedGroupDto closedGroupDto);
    }
}
