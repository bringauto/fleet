import { getTime } from "./timeHelpers";
import i18n from "../../plugins/i18n/i18n";

export function orderListing(order) {
  return `${i18n.tc("orders.to")} ${order.to.name} ${
    order.arrive ? `${i18n.tc("orders.in")} ${getTime(order.arrive)}` : i18n.tc("orders.asap")
  }`;
}
