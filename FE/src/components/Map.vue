<template>
  <div :class="{ small }" class="map">
    <l-map
      ref="map"
      :center="center"
      :zoom="zoom"
      class="map__container"
      @update:center="centerUpdated"
      @update:zoom="zoomUpdated"
    >
      <l-tile-layer :url="url" />
      <template v-for="car in cars">
        <template v-for="history in carLocationHistory(car.locationHistory.nodes)">
          <l-marker
            v-if="history.latitude && history.longitude"
            :key="history.id"
            :icon="car.underTest ? disabledHistoryIcon : historyIcon"
            :lat-lng="[history.latitude, history.longitude]"
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
            :class-name="`car-icon ${car.underTest ? 'disabled' : ''}`"
            :icon-anchor="[22.5, 22.5]"
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
          :icon="stationIcon"
          :lat-lng="[station.latitude, station.longitude]"
          :z-index-offset="2"
          @click="isAdmin ? $emit('station-clicked', station) : ''"
        >
          <l-tooltip>{{ station.name }}</l-tooltip>
        </l-marker>
      </template>
      <template v-for="route in routes">
        <l-polyline
          v-if="route"
          :key="`route${route.id}`"
          :color="route.color"
          :lat-lngs="route.stops"
          @click="$emit('route-clicked', route)"
        >
          <!-- <l-tooltip>{{ route.name }}</l-tooltip> -->
        </l-polyline>
      </template>
    </l-map>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";
import { isAfter, subMinutes } from "date-fns";
import { icon, latLng } from "leaflet";
import { LIcon, LMap, LMarker, LPolyline, LTileLayer, LTooltip } from "vue2-leaflet";
import { routeApi, stationApi } from "../code/api";
import { getLastUpdate } from "../code/helpers/timeHelpers";
import { GetterNames, MutationNames } from "../store/enums/vuexEnums";
import { RoleEnum } from "../code/enums/roleEnums";

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
    center: latLng(49.8401525, 18.2302432),
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
  computed: {
    ...mapGetters({
      getMe: GetterNames.GetMe,
      roles: GetterNames.GetRoles,
      isRole: GetterNames.isRole,
      getFirstStation: GetterNames.isFirstStation,
    }),
    ...mapMutations({
      loadTenant: MutationNames.LoadTenant,
    }),
    isAdmin() {
      return this.isRole(RoleEnum.Admin);
    },
  },
  async mounted() {
    // const { longitude, latitude } = this.getFirstStation;
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

    this.routes = await routeApi.getRoutes(true);
  },

  methods: {
    getLastUpdate,
    handleCarClick(car) {
      this.center = latLng(car.latitude, car.longitude);
      this.setLocalSettings({ selectedCar: car.id });
      this.$emit("car-clicked", car);
    },
    async centerUpdated(center) {
      this.stations = await stationApi.getStations();
      this.center = latLng(
        this.stations[0].latitude ?? 49.8401525,
        this.stations[0].longitude ?? 18.2302432
      );
      console.log(center);
      this.setLocalSettings({ center });
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
    background: url("../../public/img/activeMarker.svg") no-repeat;
    background-size: cover;
    background-position: center !important;
    line-height: 45px;
    width: 45px !important;
    height: 45px !important;
    text-align: center;
    color: white;
    transition: transform 0.2s ease;

    &.disabled {
      background: url("../../public/img/defaultMarker.svg") no-repeat;
      background-size: cover;
      background-position: center !important;
    }
  }
}
</style>
