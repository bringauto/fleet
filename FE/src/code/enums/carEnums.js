import i18n from "../../plugins/i18n/i18n";

export const CarState = Object.freeze({
  WAITING: "WAITING",
  ACCEPTIONORDER: "ACCEPTIONORDER",
  DRIVING: "DRIVING",
  WAITFORLOAD: "WAITFORLOAD",
  OUTOFSERVICE: "OUTOFSERVICE",
  CHARGING: "CHARGING",
  STOPPEDBYPHONE: "STOPPEDBYPHONE",
});

export const CarStateFormated = [
  {
    status: CarState.WAITING,
    trans: i18n.tc("statuses.waiting"),
  },
  {
    status: CarState.ACCEPTIONORDER,
    trans: i18n.tc("statuses.acceptionOrder"),
  },
  {
    status: CarState.DRIVING,
    trans: i18n.tc("statuses.driving"),
  },
  {
    status: CarState.WAITFORLOAD,
    trans: i18n.tc("statuses.waitingForLoad"),
  },
  {
    status: CarState.OUTOFSERVICE,
    trans: i18n.tc("statuses.outOfService"),
  },
  {
    status: CarState.CHARGING,
    trans: i18n.tc("statuses.charging"),
  },
  {
    status: CarState.STOPPEDBYPHONE,
    trans: i18n.tc("statuses.stoppedByPhone"),
  }
];

export const getCarState = (status) => {
  return CarStateFormated.find((s) => s.status === status);
};
