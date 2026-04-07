

using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderProfile;

public class UpdateReaderProfileCommand : IRequest<bool>
{
        public Guid UserId { get; set; }

        public string? BioVi { get; set; }

        public string? BioEn { get; set; }

        public string? BioZh { get; set; }

        public long? DiamondPerQuestion { get; set; }

        public List<string>? Specialties { get; set; }
}
