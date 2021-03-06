import { extend } from "vee-validate";
import { required } from "vee-validate/dist/rules";
import i18n from "../../plugins/i18n/i18n";

extend("required", {
  ...required,
  message: i18n.tc("validations.required"),
});

extend("station_not_equal", {
  params: ["target"],
  validate(value, { target }) {
    return value !== target;
  },
  message: i18n.tc("validations.station_not_equal"),
});
