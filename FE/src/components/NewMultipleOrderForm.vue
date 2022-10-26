<template>
  <v-col cols="12" md="8">
    <ValidationObserver v-slot="{ handleSubmit }" ref="form">
      <form novalidate @submit.prevent="handleSubmit(onSubmit)">
        <ValidationProvider
          v-slot="{ errors }"
          vid="carId"
          rules="required"
          :name="$t('newOrder.car')"
        >
          <v-select
            v-model="carId"
            :label="$t('newOrder.car')"
            :items="cars"
            item-text="name"
            item-value="id"
            required
            :error-messages="errors"
            :no-data-text="$t('cars.noCars')"
          />
          <v-select
            :items="mappedRoutes"
            :label="$t('settings.route')"
            :value="routeId"
            clearable
            hide-details
            item-text="name"
            item-value="id"
            :no-data-text="$t('routes.noRoutes')"
            @input="mappStations"
          />
        </ValidationProvider>
        <div v-if="carId" class="mb-5">
          <span class="caption">{{ $t("newOrder.check") }}</span>
          <div v-for="(station, index) in mappedStations" :key="index" class="draggable-item">
            <v-checkbox
              v-model="station.checked"
              :label="station.name"
              :true-value="true"
              :false-value="false"
              hide-details
            />
          </div>
        </div>
        <!--
        <ValidationProvider v-slot="{ errors }" :name="$t('newOrder.priority')" vid="selectedPrio">
          <v-select
            v-model="selectedPrio"
            :label="$t('newOrder.priority')"
            :items="priorities"
            item-text="trans"
            item-value="priority"
            required
            :error-messages="errors"
          ></v-select>
        </ValidationProvider>
        -->
        <div class="mt-5">
          <v-btn large color="success" class="mr-4" type="submit">
            {{ $t("login.submit") }}
          </v-btn>
        </div>
      </form>
    </ValidationObserver>
  </v-col>
</template>

<script>
import { mapGetters } from "vuex";
import { ValidationObserver, ValidationProvider } from "vee-validate";
import { formatArrive } from "../code/helpers/timeHelpers";
import { carApi, orderApi, routeApi, stationApi } from "../code/api";
import { getPrioEnumAccordingToRole } from "../code/enums/prioEnum";
import allRoutes from "../code/enums/routesEnum";
import { GetterNames } from "../store/enums/vuexEnums";
import { CarStateFormated } from "../code/enums/carEnums";
import { OrderState } from "../code/enums/orderEnums";

export default {
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  data() {
    return {
      stations: [],
      cars: [],
      priorities: [],
      stationFrom: undefined,
      arrive: null,
      stationTo: null,
      selectedPrio: null,
      carId: null,
      routes: [],
      mappedStations: [],
      routeId: null,
      CarStateFormated,
    };
  },
  computed: {
    ...mapGetters({
      roles: GetterNames.GetRoles,
      isAdmin: GetterNames.isAdmin,
      isDriver: GetterNames.isDriver,
    }),
    mappedRoutes() {
      const selectedCar = this.cars.find((car) => this.carId === car.id);
      if (selectedCar) {
        const selectedOrder = selectedCar.orders.nodes.find((order) =>
          [OrderState.ACCEPTED, OrderState.TOACCEPT, OrderState.INPROGRESS].includes(order.status)
        );
        if (selectedOrder) {
          return this.routes.filter((route) => {
            return route.stops.some(
              (stop) => stop.station && stop.station.id === selectedOrder.to.id
            );
          });
        }
      }
      return this.routes;
    },
  },
  watch: {
    cars: {
      handler(val) {
        if (val.length === 1 && !this.carId) {
          this.carId = val[0].id;
        }
      },
      deep: true,
      immediate: true,
    },
    priorities: {
      handler(val) {
        if (val.length > 0 && !this.selectedPrio) {
          this.selectedPrio = val[val.length - 1].priority;
        }
      },
      deep: true,
      immediate: true,
    },
  },
  async mounted() {
    await this.initForm();
  },
  methods: {
    async initForm() {
      this.routes = await routeApi.getRoutes();
      this.stations = await stationApi.getStations();
      const cars = await carApi.getCarsWithOrders();
      this.cars = cars.filter((car) => (car.underTest && this.isAdmin) || !car.underTest);
      this.priorities = getPrioEnumAccordingToRole(this.$store.state.user.roles);
      this.stationTo = this.$route.params.stationTo;
      this.carId = this.$route.params.carId;
    },
    formatOrder(stationTo) {
      const dto = {
        carId: this.carId,
        priority: this.selectedPrio,
        toStationId: stationTo.id,
      };
      dto.arrive = formatArrive(this.arrive);
      return dto;
    },
    mappStations(id) {
      this.routeId = id;
      if (id) {
        const selectedRoute = this.routes.find((route) => route.id === id);
        this.mappedStations = selectedRoute.stops.reduce((acc, stop) => {
          if (stop.station) {
            acc.push({ ...stop.station, checked: true });
          }
          return acc;
        }, []);
      } else {
        this.mappedStations = [];
      }
    },
    async onSubmit() {
      try {
        const { mappedStations } = this;
        // eslint-disable-next-line no-restricted-syntax
        for (const station of mappedStations) {
          if (station.checked) {
            const dto = this.formatOrder(station);
            // eslint-disable-next-line no-await-in-loop
            await orderApi.addOrder(dto);
          }
        }
        const selectedCar = this.cars.find((car) => car.id === this.carId);
        await carApi.updateCar({
          ...selectedCar,
          routeId: this.routeId,
        });

        this.$router.push({
          name: this.isAdmin ? allRoutes.Teleop : allRoutes.Dashboard,
        });
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.createMultiple"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.order.createMultipleFailed"),
          type: "error",
        });
        console.error(e);
      }
    },
  },
};
</script>

<style lang="scss">
.draggable {
  &-item {
    padding: 5px 0px;
  }

  &-ghost {
    cursor: grabbing;
    color: rgba(0, 0, 0, 0.5);
  }
}
</style>
