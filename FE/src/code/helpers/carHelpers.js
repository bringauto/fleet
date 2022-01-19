export function getCarBatteryIcon(battery) {
  const icons = [
    { icon: "battery-high", value: 70 },
    { icon: "battery-medium", value: 50 },
    { icon: "battery-low", value: 20 },
    { icon: "battery-outline", value: 0 },
  ];
  const found = icons.find((item) => item.value <= battery);
  return `mdi-${found.icon}`;
}
