<template>
  <div class="dashboard">
    <Map :cars="cars" @car-clicked="handleClickCar" @station-clicked="handleClickSation" />
    <v-fade-transition>
      <CarCard
        class="dashboard__card"
        :car="selectedCar"
        @set-car-status="updateSelectedCar"
        @set-order-status="updateSelectedOrder"
        @get-cars="getAllCars()"
      />
    </v-fade-transition>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { carApi, orderApi } from "../code/api";
import CarCard from "../components/TeleopCard.vue";
import { GetterNames } from "../store/enums/vuexEnums";
import { CarStateFormated } from "../code/enums/carEnums";
import Map from "../components/Map.vue";
import { DOWNLOAD_INTERVAL } from "../code/enums/constants";
import allRoutes from "../code/enums/routesEnum";

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
    CarStateFormated,
  }),
  computed: {
    ...mapGetters({
      roles: GetterNames.GetRoles,
      isAdmin: GetterNames.isAdmin,
    }),
  },
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
    this.pollData();
    await this.getAllCars();
    try {
      const settings = JSON.parse(localStorage.getItem("mapSettings"));
      if (settings?.selectedCar) {
        this.selectedCar =
          this.cars.find((car) => car.id === Number(settings.selectedCar)) ?? this.cars[0];
      } else {
        [this.selectedCar] = this.cars;
      }
    } catch (e) {
      console.error(e);
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

    async updateSelectedOrder(obj) {
      const dto = {
        car: obj.car.id,
        id: obj.order.id,
        priority: obj.order.priority,
        status: obj.status,
        to: obj.order.to.id,
      };
      dto.from = obj?.order?.from?.id;
      dto.arrive = obj?.arrive;
      try {
        await orderApi.updateOrder(dto);
        await this.getAllCars();
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.update"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.updateFailed"),
          type: "error",
        });
        console.error(e);
      }
    },
    async updateSelectedCar(obj) {
      const dto = {
        ...obj,
        id: obj.car.id,
        name: obj.car.name,
        status: obj.status,
        underTest: obj.underTest ?? false,
      };
      try {
        await carApi.updateCar(dto);
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.car.update"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.car.updateFailed"),
          type: "error",
        });
        console.error(e);
      }
      await this.getAllCars();
    },
    pollData() {
      this.polling = setInterval(() => {
        this.getAllCars();
      }, DOWNLOAD_INTERVAL);
    },
    handleClickCar(car) {
      this.selectedCar = car;
    },
    handleClickSation() {
      this.$router.push({ name: allRoutes.Settings });
    },
  },
};
</script>
