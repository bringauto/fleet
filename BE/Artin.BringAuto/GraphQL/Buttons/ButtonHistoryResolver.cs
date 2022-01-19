using Artin.BringAuto.Shared.Butons;
using Artin.BringAuto.Shared.Cars;
using HotChocolate;
using System.Linq;

namespace Artin.BringAuto.GraphQL.Buttons
{
    public class ButtonHistoryResolver
    {
        public IQueryable<Button> GetCarButtonHistory([Parent] Car car, [Service] IButtonRepository buttonHistoryRepository)
      => buttonHistoryRepository.GetButtonsByCar(car.Id);
    }
}
