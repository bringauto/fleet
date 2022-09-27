export function getCarBatteryIcon(battery) {
  const icons = [
    { icon: "battery-high", value: 0.7 },
    { icon: "battery-medium", value: 0.5 },
    { icon: "battery-low", value: 0.2 },
    { icon: "battery-outline", value: 0 },
  ];
  const found = icons.find((item) => item.value <= battery);
  return `mdi-${found.icon}`;
}
