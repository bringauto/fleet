<template>
  <v-form>
    <v-container>
      <v-row>
        <v-col cols="12" md="6">
          <ValidationProvider v-slot="{ errors }" :name="$t('general.name')" rules="required">
            <v-text-field
              :error-messages="errors"
              :label="$t('general.name')"
              :value="car.name"
              required
              @input="$emit('update:car', { ...car, name: $event })"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="6">
          <v-text-field
            :label="$t('cars.hwId')"
            :value="car.hwId"
            @input="$emit('update:car', { ...car, hwId: $event })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-text-field
            :label="$t('cars.companyName')"
            :value="(car.companyName = getTenant.name.toLowerCase())"
            readonly
            disabled
            @input="$emit('update:car', { ...car, companyName: getTenant.name.toLowerCase() })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-text-field
            :label="$t('cars.carAdminPhone')"
            :value="car.carAdminPhone"
            @input="$emit('update:car', { ...car, carAdminPhone: $event })"
            @keydown="justNumber"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-select
            :items="mappedRoutes"
            :label="$t('settings.route')"
            :value="car.routeId"
            clearable
            disabled
            append-icon="dropdown"
            hide-details
            item-text="name"
            item-value="id"
            @input="$emit('update:car', { ...car, routeId: $event })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-select
            :items="CarStateFormated"
            :label="$t('general.status')"
            :value="car.status"
            hide-details
            item-text="trans"
            item-value="status"
            @input="$emit('update:car', { ...car, status: $event })"
          />
        </v-col>
        <v-col cols="12" md="6">
          <v-checkbox
            :false-value="false"
            :input-value="car.underTest"
            :label="$t('cars.underTest')"
            :true-value="true"
            @change="$emit('update:car', { ...car, underTest: $event })"
          />
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>

<script>
import { mapGetters } from "vuex";
import { ValidationProvider } from "vee-validate";
import { carApi, routeApi, stationApi } from "../../code/api";
import { GetterNames } from "../../store/enums/vuexEnums";
import { CarStateFormated } from "../../code/enums/carEnums";
import { OrderState } from "../../code/enums/orderEnums";
import { justNumber } from "../../code/helpers/positionHelpers";

export default {
  name: "EditCarModal",
  components: {
    ValidationProvider,
  },
  props: {
    car: {
      type: Object,
      required: true,
    },
    routes: {
      type: Array,
      default: () => {
        return [];
      },
    },
  },
  data: () => ({
    CarStateFormated,
    stations: [],
    cars: [],
    carId: null,
    route: [],
  }),

  computed: {
    ...mapGetters({
      getTenant: GetterNames.GetTenant,
    }),
    mappedRoutes() {
      const selectedCar = this.cars.find((car) => this.carId === car.id);
      if (selectedCar) {
        const selectedOrder = selectedCar.orders.nodes.find((order) =>
          [OrderState.ACCEPTED, OrderState.TOACCEPT, OrderState.INPROGRESS].includes(order.status)
        );
        if (selectedOrder) {
          return this.route.filter((route) =>
            route.stops.some((stop) => stop.station && stop.station.id === selectedOrder.to.id)
          );
        }
      }
      return this.route;
    },
  },
  async mounted() {
    await this.initForm();
  },
  methods: {
    async initForm() {
      this.route = await routeApi.getRoutes();
      this.stations = await stationApi.getStations();
      const cars = await carApi.getCarsWithOrders();
      this.carId = this.car.id;
      this.cars = cars;
    },
    justNumber,
  },
};
</script>
