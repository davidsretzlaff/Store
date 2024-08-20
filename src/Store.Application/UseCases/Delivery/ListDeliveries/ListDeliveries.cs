
using Store.Application.UseCases.Delivery.Common;
using Store.Application.UseCases.User.Common;
using Store.Application.UseCases.User.ListUsers;
using Store.Domain.Interface.Application;
using Store.Domain.Interface.Infra.Repository;

namespace Store.Application.UseCases.Delivery.ListDeliveries
{
	public class ListDeliveries : IListDeliveries
	{
		private readonly IDeliveryRepository _deliveryRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserValidation _userValidation;

		public ListDeliveries
		(
			IDeliveryRepository deliveryRepository,
			IOrderRepository orderRepository,
			IUnitOfWork unitOfWork,
			IUserValidation userValidation
		)
		{
			_deliveryRepository = deliveryRepository;
			_unitOfWork = unitOfWork;
			_userValidation = userValidation;
		}

		public async Task<ListDeliveriesOutput> Handle(ListDeliveriesInput input, CancellationToken cancellationToken)
		{
			await _userValidation.IsUserActive(input.User, cancellationToken);
			var searchOutput = await _deliveryRepository.Search(
				new Domain.SeedWork.Searchable.SearchInput(
						input.Page,
						input.PerPage,
						input.Search,
						input.OrderBy,
						input.Order,
						input.User
					),
					cancellationToken
				);

			return new ListDeliveriesOutput(
				searchOutput.CurrentPage,
				searchOutput.PerPage,
				searchOutput.Total,
				searchOutput.Items.Select(DeliveryOutput.FromDelivery).ToList()
				);
		}
	}
}
