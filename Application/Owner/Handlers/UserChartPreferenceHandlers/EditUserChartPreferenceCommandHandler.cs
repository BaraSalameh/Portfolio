using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserChartPreferenceCommands;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserChartPreferenceHandlers
{
    public class EditUserChartPreferenceCommandHandler : IRequestHandler<EditUserChartPreferenceCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public EditUserChartPreferenceCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(EditUserChartPreferenceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var widgetExists = await _context.LKP_Widget.AsNoTracking().AnyAsync(
                widget => widget.ID == request.LKP_WidgetID,
                cancellationToken);
            var chartTypeExists = await _context.LKP_ChartType.AsNoTracking().AnyAsync(
                chartType => chartType.ID == request.LKP_ChartTypeID,
                cancellationToken);
            if (!widgetExists || !chartTypeExists)
            {
                response.lstError.Add("Widget or chart type not found.");
                return response;
            }

            var existingEntity = await _context.UserChartPreference
                .FirstOrDefaultAsync(x =>
                    x.UserID == _currentUser.UserID!.Value &&
                    x.LKP_WidgetID == request.LKP_WidgetID &&
                    x.LKP_ChartTypeID == request.LKP_ChartTypeID &&
                    x.IsDeleted == false,
                    cancellationToken
                );

            if (existingEntity == null)
            {
                var newEntity = _mapper.Map<UserChartPreference>(request);
                newEntity.UserID = _currentUser.UserID!.Value;
                await _context.UserChartPreference.AddAsync(newEntity, cancellationToken);
            }
            else
            {
                _mapper.Map(request, existingEntity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
