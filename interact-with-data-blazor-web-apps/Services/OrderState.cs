using BlazingPizza.Model;

namespace BlazingPizza.Services
{
	public class OrderState
	{
		public bool ShowingConfigureDialog { get; private set; }
		public Pizza ConfiguringPizza { get; private set; }
		public Order Order { get; private set; } = new Order();

		public void ShowConfigurePizzaDialog(PizzaSpecial special)
		{
			ConfiguringPizza = new Pizza()
			{
				Special = special,
				SpecialId = special.Id,
				Size = Pizza.DefaultSize,
				Toppings = new List<PizzaTopping>(),
			};
			Console.WriteLine(ConfiguringPizza.GetHashCode());
			//누를때마다 객체가 새로 생기는 형태이다.
			ShowingConfigureDialog = true;
		}

		public void CancelConfigurePizzaDialog()
		{
			ConfiguringPizza = null;

			ShowingConfigureDialog = false;
		}

		public void ConfirmConfigurePizzaDialog()
		{
			Order.Pizzas.Add(ConfiguringPizza);
			ConfiguringPizza = null;

			ShowingConfigureDialog = false;
		}

		public void RemoveConfiguredPizza(Pizza pizza)
		{
			Order.Pizzas.Remove(pizza);
		}
	}
}
