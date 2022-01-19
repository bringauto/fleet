import i18n from "../../plugins/i18n/i18n";
import { RoleEnum } from "./roleEnums";

export const PriorityEnum = Object.freeze({
  LOW: "LOW",
  NORMAL: "NORMAL",
  HIGH: "HIGH",
});

export const PriorityEnumFormated = [
  {
    priority: PriorityEnum.LOW,
    trans: i18n.tc("priority.low"),
    value: -1,
    color: "success",
  },
  {
    priority: PriorityEnum.NORMAL,
    trans: i18n.tc("priority.normal"),
    value: 0,
    color: "primary",
  },
  {
    priority: PriorityEnum.HIGH,
    trans: i18n.tc("priority.high"),
    value: 1,
    color: "error",
  },
];

export const getPriorityEnum = (priority) => {
  return PriorityEnumFormated.find((s) => s.priority === priority);
};

export const getPrioEnumAccordingToRole = (role) => {
  if (role.includes(RoleEnum.Admin) || role.includes(RoleEnum.Privileged)) {
    return PriorityEnumFormated;
  }
  return PriorityEnumFormated.filter((p) => p.priority !== PriorityEnum.HIGH);
};
