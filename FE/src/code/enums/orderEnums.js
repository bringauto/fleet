import i18n from "../../plugins/i18n/i18n";

export const OrderState = Object.freeze({
  ACCEPTED: "ACCEPTED",
  INPROGRESS: "INPROGRESS",
  DONE: "DONE",
  CANCELED: "CANCELED",
  TOACCEPT: "TOACCEPT",
});

export const OrderStateFormated = [
  {
    status: OrderState.TOACCEPT,
    trans: i18n.tc("satuses.toAccept"),
  },
  {
    status: OrderState.ACCEPTED,
    trans: i18n.tc("satuses.accepted"),
  },
  {
    status: OrderState.INPROGRESS,
    trans: i18n.tc("satuses.inProgress"),
  },
  {
    status: OrderState.DONE,
    trans: i18n.tc("satuses.done"),
  },
  {
    status: OrderState.CANCELED,
    trans: i18n.tc("satuses.canceled"),
  },
];

export const getOrderState = (status) => {
  return OrderStateFormated.find((s) => s.status === status);
};
