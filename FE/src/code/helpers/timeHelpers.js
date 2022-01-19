import { format, startOfToday, add } from "date-fns";
import i18n from "../../plugins/i18n/i18n";

export function getTime(date, dateFormat = "k:mm") {
  return format(new Date(date), dateFormat);
}

export function getLastUpdate(car) {
  const history = car.locationHistory.nodes;
  const update = history && history.length > 0 ? history[history.length - 1].dateTime : undefined;
  return update ? getTime(update, "d.M.y k:mm") : i18n.tc("general.carUpdate");
}

export function formatArrive(arrive) {
  if (arrive) {
    const time = arrive.split(":");
    return add(startOfToday(), { hours: time[0], minutes: time[1] });
  }
  return undefined;
}
