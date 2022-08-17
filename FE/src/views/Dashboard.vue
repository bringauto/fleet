<template>
  <div class="dashboard">
    <Map :cars="cars" @car-clicked="handleClickCar" @station-clicked="handleClickSation" />
    <v-fade-transition>
      <CarCard :car="selectedCar" class="dashboard__card" @get-cars="getAllCars()" />
    </v-fade-transition>
  </div>
</template>

<script>
import CarCard from "../components/DashboardCard.vue";
import Map from "../components/Map.vue";
import { DOWNLOAD_INTERVAL } from "../code/enums/constants";
import allRoutes from "../code/enums/routesEnum";
import { carApi } from "../code/api";

export default {
  name: "Home",
  components: {
    CarCard,
    Map,
  },
  data: () => ({
    polling: null,
    cars: null,
    selectedCar: undefined,
    stations: null,
  }),
  watch: {
    cars: {
      handler(value) {
        if (value && this.selectedCar?.id) {
          this.selectedCar = value.find((car) => car.id === this.selectedCar.id);
        }
      },
      deep: true,
      immediate: true,
    },
  },
  async mounted() {
    await this.getAllCars();
    this.pollData();
    try {
      const settings = JSON.parse(localStorage.getItem("mapSettings"));
      if (settings?.selectedCar) {
        this.selectedCar =
          this.cars.find((car) => car.id === Number(settings.selectedCar)) ?? this.cars[0];
      } else {
        [this.selectedCar] = this.cars;
      }
    } catch (e) {
      this.$notify({
        group: "global",
        type: "error",
        text: e,
      });
    }
  },
  beforeDestroy() {
    clearInterval(this.polling);
  },
  methods: {
    async getAllCars() {
      try {
        this.cars = await carApi.getCarsWithOrders();
      } catch (e) {
        this.$notify({
          group: "global",
          type: "error",
          text: e,
        });
      }
    },
    pollData() {
      this.polling = setInterval(() => {
        this.getAllCars();
      }, DOWNLOAD_INTERVAL);
    },
    handleClickCar(car) {
      this.selectedCar = car;
    },
    handleClickStation(station) {
      this.$router.push({ name: allRoutes.NewOrder, params: { stationTo: station.id } });
    },
  },
};
</script>

<style lang="scss">
.dashboard {
  position: relative;

  &__card {
    position: absolute;
    right: 20px;
    bottom: 20px;
    @media (max-width: 768px) {
      position: relative;
      right: inherit;
      bottom: inherit;
    }
  }
}
</style>
