
# Order

Order represents order that must be fulfilled

- `arrive: DateTime`. When the car should arrive/fulfill the order.
- `car: Car`. The [Car] to which the Order is assigned
- `to: Station`. The [Station] TO which the car should go.
- `toStationPhone: string`.
- `from: Station`. The [Station] FROM which the car should start
- `fromStationPhone: string`. 
- `priority: OrderPriority`. Priority of the order. OrderPriority = { LOW, NORMAL, HIGH }
- `status: OrderStatus`. OrderStatus = { TOACCEPT, ACCEPTED, INPROGRESS, DONE, CANCELED }
- `user: User`. [User] who's made the order

In GUI the order can be created by [SimpleOrder] or [GroupOrder]


[Car]: ./Car.md
[Station]: ./Station.md
[User]: ./User.md
[SimpleOrder]: ./SimpleOrder.md
[GroupOrder]: ./GroupOrder.md