<template>
  <v-col cols="12" md="8">
    <ValidationObserver ref="form" v-slot="{ handleSubmit }">
      <form novalidate @submit.prevent="handleSubmit(onSubmit)">
        <ValidationProvider
          v-slot="{ errors }"
          :name="$t('newOrder.car')"
          rules="required"
          vid="carId"
        >
          <v-select
            v-model="carId"
            :error-messages="errors"
            :items="cars"
            :label="$t('newOrder.car')"
            item-text="name"
            item-value="id"
            required
          ></v-select>
        </ValidationProvider>
        <div v-if="carId" class="mb-5">
          <span class="caption">{{ $t("newOrder.check") }}</span>
          <div v-for="(station, index) in mappedStations" :key="index" class="draggable-item">
            <v-checkbox
              v-model="station.checked"
              :false-value="false"
              :label="station.name"
              :true-value="true"
              hide-details
            />
          </div>
        </div>
        <ValidationProvider v-slot="{ errors }" :name="$t('newOrder.priority')" vid="selectedPrio">
          <v-select
            v-model="selectedPrio"
            :error-messages="errors"
            :items="priorities"
            :label="$t('newOrder.priority')"
            item-text="trans"
            item-value="priority"
            required
          ></v-select>
        </ValidationProvider>
        <div class="mt-5">
          <v-btn class="mr-4" color="success" large type="submit">
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

export default {
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  data() {
    return {
      stations: [],
      mappedStations: [],
      cars: [],
      routes: [],
      priorities: [],
      stationFrom: undefined,
      arrive: null,
      stationTo: null,
      selectedPrio: null,
      carId: null,
    };
  },
  computed: {
    ...mapGetters({
      roles: GetterNames.GetRoles,
      isAdmin: GetterNames.isAdmin,
    }),
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
    carId: {
      handler(val) {
        if (val) {
          const { routeId } = this.cars.find((car) => car.id === val);

          if (routeId) {
            const selectedRoute = this.routes.find((route) => route.id === routeId);
            this.mappedStations = selectedRoute.stops.reduce((acc, stop) => {
              if (stop.station) {
                acc.push({ ...stop.station, checked: true });
              }
              return acc;
            }, []);
          }
        } else {
          this.mappedStations = this.stations;
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
      const cars = await carApi.getCarsWithoutHistory();
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
        this.$router.push({ name: this.isAdmin ? allRoutes.Teleop : allRoutes.Dashboard });
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
          text: e,
        });
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
