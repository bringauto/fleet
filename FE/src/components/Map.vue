<template>
  <div class="map" :class="{ small }">
    <l-map
      ref="map"
      class="map__container"
      :zoom="zoom"
      :center="center"
      @update:center="centerUpdated"
      @update:zoom="zoomUpdated"
    >
      <l-tile-layer :url="url" />
      <template v-for="car in cars">
        <template v-for="history in carLocationHistory(car.locationHistory.nodes)">
          <l-marker
            v-if="history.latitude && history.longitude"
            :key="history.id"
            :lat-lng="[history.latitude, history.longitude]"
            :icon="car.underTest ? disabledHistoryIcon : historyIcon"
            :z-index-offset="0"
          />
        </template>
        <l-marker
          v-if="car.latitude && car.longitude"
          :key="car.id"
          :lat-lng="[car.latitude, car.longitude]"
          :z-index-offset="3"
          @click="handleCarClick(car)"
        >
          <l-icon
            :icon-anchor="[22.5, 22.5]"
            :class-name="`car-icon ${car.underTest ? 'disabled' : ''}`"
          >
            <span> {{ car.hwId }}</span>
          </l-icon>
          <l-tooltip>{{ car.name }} - {{ getLastUpdate(car) }}</l-tooltip>
        </l-marker>
      </template>
      <template v-for="station in stations">
        <l-marker
          v-if="station.latitude && station.longitude"
          :key="`station${station.id}`"
          :lat-lng="[station.latitude, station.longitude]"
          :icon="stationIcon"
          :z-index-offset="2"
          @click="$emit('station-clicked', station)"
        >
          <l-tooltip>{{ station.name }}</l-tooltip>
        </l-marker>
      </template>
      <template v-for="route in routes">
        <l-polyline
          v-if="route"
          :key="`route${route.id}`"
          :lat-lngs="route.stops"
          :color="route.color"
          @click="$emit('route-clicked', route)"
        >
          <!-- <l-tooltip>{{ route.name }}</l-tooltip> -->
        </l-polyline>
      </template>
    </l-map>
  </div>
</template>

<script>
import { isAfter, subMinutes } from "date-fns";
import { latLng, icon } from "leaflet";
import { LMap, LTileLayer, LMarker, LIcon, LTooltip, LPolyline } from "vue2-leaflet";
import { stationApi, routeApi } from "../code/api";
import { getLastUpdate } from "../code/helpers/timeHelpers";

export default {
  name: "Map",
  components: {
    LMap,
    LTileLayer,
    LMarker,
    LTooltip,
    LPolyline,
    LIcon,
  },
  props: {
    cars: { type: Array, default: () => [] },
    small: { type: Boolean, default: false },
  },
  data: () => ({
    zoom: 15,
    center: latLng(51.50337, -0.113511),
    url: "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
    selectedCar: null,
    stationIcon: icon({
      iconUrl: "./img/stationMarker.svg",
      iconSize: [39, 39],
      iconAnchor: [19.5, 19.5],
      popupAnchor: [19.5, 0],
    }),
    defaultIcon: icon({
      iconUrl: "./img/activeMarker.svg",
      iconSize: [39, 39],
      iconAnchor: [19.5, 19.5],
      popupAnchor: [19.5, 0],
      className: "car-icon",
    }),
    historyIcon: icon({
      iconUrl: "./img/historyMarker.svg",
      iconSize: [10, 10],
      iconAnchor: [5, 5],
      popupAnchor: [5, 0],
    }),
    disabledHistoryIcon: icon({
      iconUrl: "./img/disabledHistoryMarker.svg",
      iconSize: [10, 10],
      iconAnchor: [5, 5],
      popupAnchor: [5, 0],
    }),
    stations: [],
    routes: [],
  }),
  async mounted() {
    try {
      const settings = JSON.parse(localStorage.getItem("mapSettings"));
      if (settings) {
        Object.keys(settings).forEach((setting) => {
          this[setting] = settings[setting];
        });
      }
    } catch (e) {
      console.error(e);
    }
    this.stations = await stationApi.getStations();
    this.routes = await routeApi.getRoutes(true);
  },
  methods: {
    getLastUpdate,
    handleCarClick(car) {
      this.center = latLng(car.latitude, car.longitude);
      this.setLocalSettings({ selectedCar: car.id });
      this.$emit("car-clicked", car);
    },
    centerUpdated(center) {
      this.setLocalSettings({ center });
      this.center = center;
    },
    zoomUpdated(zoom) {
      this.setLocalSettings({ zoom });
      this.zoom = zoom;
    },
    setLocalSettings(settings) {
      const oldSettings = JSON.parse(localStorage.getItem("mapSettings"));
      localStorage.setItem(
        "mapSettings",
        JSON.stringify(oldSettings ? { ...oldSettings, ...settings } : settings)
      );
    },
    carLocationHistory(nodes) {
      const points = [...nodes].reverse();
      const lastMin = subMinutes(new Date(), 2);
      const history = points.filter((hist) => {
        return !isAfter(lastMin, new Date(hist.dateTime));
      });
      if (history.length > 0) {
        return history;
      }
      return points.slice(1, 3);
    },
  },
};
</script>

<style lang="scss">
.map {
  position: relative;
  height: calc(100vh - 64px) !important;
  z-index: 0;

  padding: 10px;
  &__button {
    z-index: 999;
    position: absolute;
    right: 20px;
    bottom: 20px;
  }
  &.small {
    height: calc(80vh - 64px) !important;
  }
  &__container {
    box-shadow: 3px 2px 10px rgba(0, 0, 0, 0.1);
    border-radius: 15px;
  }
  .car-icon {
    background: url("/img/activeMarker.svg") no-repeat;
    background-size: cover;
    background-position: center !important;
    line-height: 45px;
    width: 45px !important;
    height: 45px !important;
    text-align: center;
    color: white;
    transition: transform 0.2s ease;
    &.disabled {
      background: url("/img/defaultMarker.svg") no-repeat;
      background-size: cover;
      background-position: center !important;
    }
  }
}
</style>
