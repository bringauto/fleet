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

extend("coordinates_validation", {
  validate: (value) => {
    console.log(value);
    const coordinates = value.split(/\s?[,]\s?/g);
    console.log("split coordinates: ", coordinates);

    if (coordinates.length !== 2) {
      console.log("coordinates.length !== 2, returning FALSE");
      return false;
    }

    const trimmedCoordinates = coordinates.map((coord) => coord.trim());
    console.log("trimmed coordinates: ", trimmedCoordinates);

    const isValidCoordinate = (coord) => {
      const floatCoord = parseFloat(coord);
      console.log("is valid: ", !Number.isNaN(floatCoord));
      return !Number.isNaN(floatCoord);
    };

    return (
      trimmedCoordinates.every(isValidCoordinate) &&
      !trimmedCoordinates.some((coord) => coord === "")
    );
  },
  message: i18n.tc("validations.coordinates_incorrect_format"),
});
