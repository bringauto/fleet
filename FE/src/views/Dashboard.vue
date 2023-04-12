<template>
  <div class="dashboard">
    <Map :cars="cars" @car-clicked="handleClickCar" @station-clicked="handleClickStation" />
    <v-fade-transition>
      <CarCard
        :car="selectedCar"
        :cars="cars"
        class="dashboard__card"
        @get-cars="getAllCars()"
        @setCar="handleSetCar"
      />
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
    cars: [],
    selectedCarId: undefined,
    stations: null,
  }),
  computed: {
    selectedCar() {
      return this.cars.find((car) => car.id === this.selectedCarId);
    },
  },
  async mounted() {
    await this.getAllCars();
    this.pollData();
    try {
      const settings = JSON.parse(localStorage.getItem("mapSettings"));
      if (settings?.selectedCar) {
        this.selectedCarId = Number(settings.selectedCar) ?? this.cars[0]?.id ?? undefined;
      } else {
        this.selectedCarId = this.cars[0]?.id;
      }
    } catch (e) {
      this.$notify({
        group: "global",
        type: "error",
      });
      console.log(e);
    }
  },
  beforeDestroy() {
    clearInterval(this.polling);
  },
  methods: {
    handleSetCar(carId) {
      if (carId !== this.selectedCarId) {
        this.selectedCarId = carId;
      }
    },
    async getAllCars() {
      try {
        this.cars = await carApi.getCarsWithOrders();
      } catch (e) {
        this.$notify({
          group: "global",
          type: "error",
        });
        console.log(e);
      }
    },
    pollData() {
      this.polling = setInterval(() => {
        this.getAllCars();
      }, DOWNLOAD_INTERVAL);
    },
    handleClickCar(car) {
      this.selectedCarId = car.id;
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
