<template>
  <PageWrapper>
    <template #title>
      {{ $t("settings.title") }}
    </template>
    <h2 class="text-h5 mb-3">{{ $t("settings.stations") }}</h2>
    <v-col cols="12" md="8" align="center" class="mb-3">
      <template v-if="stations !== null">
        <v-data-table
          hide-default-footer
          :headers="headers"
          :items="stations"
          :items-per-page="-1"
          class="mt-2 mb-5"
        >
          <template #[`item.actions`]="{ item }">
            <v-btn
              small
              color="primary"
              class="mr-2"
              icon
              @click="handleEditModal('Station', item)"
            >
              <v-icon small> mdi-pencil</v-icon>
            </v-btn>
            <v-btn small color="error" icon @click="handleRemoveStation(item.id)">
              <v-icon small> mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-data-table>
      </template>
      <v-skeleton-loader v-else type="table-thead, table-tbody" />
    </v-col>
    <v-btn color="success" class="mb-8" @click="handleEditModal('Station')">
      <v-icon small class="mr-2">mdi-plus</v-icon>
      {{ $t("settings.add") }}
    </v-btn>
    <h2 class="text-h5 mb-3">{{ $t("settings.routes") }}</h2>
    <v-col cols="12" md="8" align="center" class="mb-3">
      <template v-if="routes !== null">
        <v-data-table
          hide-default-footer
          :headers="routeHeaders"
          :items="routes"
          :items-per-page="-1"
          class="mt-2 mb-5"
        >
          <template #[`item.stops`]="{ item }">
            <span v-if="item.stops && item.stops.length">{{ item.stops.length }}</span>
          </template>
          <template #[`item.color`]="{ item }">
            <div v-if="item.color" class="color-circle" :style="`background-color:${item.color}`" />
          </template>
          <template #[`item.actions`]="{ item }">
            <v-btn small color="primary" class="mr-2" icon @click="handleEditModal('Route', item)">
              <v-icon small> mdi-pencil</v-icon>
            </v-btn>
            <v-btn small color="error" icon @click="handleRemoveRoute(item.id)">
              <v-icon small> mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-data-table>
      </template>
      <v-skeleton-loader v-else type="table-thead, table-tbody" />
    </v-col>
    <v-btn color="success" class="mb-8" @click="handleEditModal('Route')">
      <v-icon small class="mr-2">mdi-plus</v-icon>
      {{ $t("settings.addRoute") }}
    </v-btn>
    <h2 class="text-h5 mb-3">{{ $t("settings.cars") }}</h2>
    <v-col cols="12" md="8" align="center" class="mb-3">
      <template v-if="cars !== null">
        <v-data-table
          hide-default-footer
          :headers="carHeaders"
          :items="cars"
          :items-per-page="-1"
          class="mt-2 mb-5"
        >
          <template #[`item.actions`]="{ item }">
            <v-btn small color="primary" class="mr-2" icon @click="handleEditModal('Car', item)">
              <v-icon small> mdi-pencil</v-icon>
            </v-btn>
            <v-btn small color="error" icon @click="handleRemoveCar(item.id)">
              <v-icon small> mdi-delete</v-icon>
            </v-btn>
          </template>
        </v-data-table>

        <v-dialog :value="!!modal && !!entity" width="700" @input="resetModal()">
          <ValidationObserver v-slot="{ invalid }">
            <v-card>
              <v-card-title class="headline primary white--text">
                {{ dialogTitle }}
              </v-card-title>

              <v-card-text>
                <EditCarModal v-if="modal === 'Car'" :car.sync="entity" :routes="routes" />
                <EditRouteModal
                  v-else-if="modal === 'Route'"
                  :route.sync="entity"
                  :stations="stations"
                />
                <EditStationModal v-else :station.sync="entity" />
              </v-card-text>

              <v-divider></v-divider>

              <v-card-actions>
                <v-spacer />

                <v-btn color="error" text @click="resetModal()">
                  {{ $t("general.cancel") }}
                </v-btn>
                <v-btn
                  color="success"
                  text
                  :disabled="
                    invalid ||
                    (!isUniqNameStation && modal === 'Station') ||
                    (!isUniqNameRoute && modal === 'Route') ||
                    (!isUniqNameCar && modal === 'Car')
                  "
                  @click="handleSave()"
                >
                  {{ $t("settings.save") }}
                </v-btn>
              </v-card-actions>
            </v-card>
          </ValidationObserver>
        </v-dialog>
      </template>
      <v-skeleton-loader v-else type="table-thead, table-tbody" />
    </v-col>
    <v-btn color="success" @click="handleEditModal('Car')">
      <v-icon small class="mr-2">mdi-plus</v-icon>
      {{ $t("settings.addCar") }}
    </v-btn>
  </PageWrapper>
</template>

<script>
import { ValidationObserver } from "vee-validate";
import { cloneDeep } from "lodash";
import { carApi, stationApi, routeApi } from "../code/api";
import EditStationModal from "../components/Editation/EditStationModal.vue";
import EditRouteModal from "../components/Editation/EditRouteModal.vue";
import EditCarModal from "../components/Editation/EditCarModal.vue";
import PageWrapper from "../components/Layout/PageWrapper.vue";
import { DEFAULT_STATION, DEFAULT_ROUTE, DEFAULT_CAR } from "../code/enums/constants";

export default {
  components: {
    ValidationObserver,
    EditStationModal,
    EditRouteModal,
    PageWrapper,
    EditCarModal,
  },
  data() {
    return {
      stations: null,
      routes: null,
      cars: null,
      editation: false,
      modal: undefined,
      entity: undefined,
      headers: [
        {
          text: this.$i18n.tc("tables.name"),
          value: "name",
        },
        {
          text: this.$i18n.tc("tables.actions"),
          value: "actions",
          sortable: false,
        },
      ],
      routeHeaders: [
        {
          text: this.$i18n.tc("tables.name"),
          value: "name",
        },
        {
          text: this.$i18n.tc("tables.color"),
          value: "color",
          sortable: false,
        },
        {
          text: this.$i18n.tc("tables.points"),
          value: "stops",
          sortable: false,
        },
        {
          text: this.$i18n.tc("tables.actions"),
          value: "actions",
          sortable: false,
        },
      ],
      carHeaders: [
        {
          text: this.$i18n.tc("tables.name"),
          value: "name",
        },
        {
          text: this.$i18n.tc("cars.hwId"),
          value: "hwId",
        },
        {
          text: this.$i18n.tc("cars.companyName"),
          value: "companyName",
        },
        {
          text: this.$i18n.tc("tables.actions"),
          value: "actions",
          sortable: false,
        },
      ],
    };
  },
  computed: {
    dialogTitle() {
      if (this.modal) {
        return this.$i18n.tc(
          this.editation ? `settings.edit${this.modal}` : `settings.create${this.modal}`
        );
      }
      return "";
    },
    isUniqNameStation() {
      console.log(this.entity);
      if (this.entity?.name) {
        return !this.stations.some(
          (station) =>
            station.name.toLowerCase() === this.entity.name.toLowerCase() &&
            station.id !== this.entity.id
        );
      }
      return true;
    },
    isUniqNameCar() {
      console.log(this.entity);
      if (this.entity?.name) {
        return !this.cars.some(
          (car) =>
            car.name.toLowerCase() === this.entity.name.toLowerCase() && car.id !== this.entity.id
        );
      }
      return true;
    },
    isUniqNameRoute() {
      console.log(this.entity);
      if (this.entity?.name) {
        return !this.routes.some(
          (route) =>
            route.name.toLowerCase() === this.entity.name.toLowerCase() &&
            route.id !== this.entity.id
        );
      }
      return true;
    },
  },
  async mounted() {
    this.getAllStations();
    this.getAllRoutes();
    this.getAllCars();
  },
  methods: {
    resetModal() {
      this.modal = undefined;
    },
    handleSave() {
      switch (this.modal) {
        case "Station":
          this.handleStationUpdate();
          break;
        case "Route":
          this.handleRouteUpdate();
          break;
        default:
          this.handleCarUpdate();
          break;
      }
    },
    async getAllStations() {
      try {
        this.stations = await stationApi.getStations();
      } catch (e) {
        this.$notify({
          group: "global",
          type: "error",
          text: e,
        });
      }
    },
    async getAllRoutes() {
      try {
        this.routes = await routeApi.getRoutes();
      } catch (e) {
        this.$notify({
          group: "global",
          type: "error",
          text: e,
        });
      }
    },
    async getAllCars() {
      try {
        this.cars = await carApi.getCarsWithoutHistory();
      } catch (e) {
        this.$notify({
          group: "global",
          type: "error",
        });
        console.log(e);
      }
    },
    async handleRemoveRoute(id) {
      try {
        await routeApi.deleteRoute(id);
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.route.removed"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.route.removeFiled"),
          type: "error",
        });
        console.error(e);
      }
      await this.getAllRoutes();
    },
    async handleRemoveStation(id) {
      try {
        await stationApi.deleteStation(id);
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.station.removed"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.station.removeFiled"),
          type: "error",
        });
        console.error(e);
      }
      await this.getAllStations();
    },
    async handleRemoveCar(id) {
      try {
        await carApi.deleteCar(id);
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.car.delete"),
          type: "success",
        });
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("notifications.car.deleteFailed"),
          type: "error",
        });
        console.error(e);
      }
      await this.getAllCars();
    },
    handleEditModal(type, entity) {
      this.editation = !!entity;
      this.modal = type;
      this.entity = entity ? cloneDeep(entity) : this.getDefaultValue(type);
    },
    getDefaultValue(type) {
      if (type === "Station") {
        return cloneDeep(DEFAULT_STATION);
      }
      if (type === "Route") {
        return cloneDeep(DEFAULT_ROUTE);
      }
      return cloneDeep(DEFAULT_CAR);
    },
    onlyNumber($event) {
      // console.log($event.keyCode); //keyCodes value
      const keyCode = $event.keyCode ? $event.keyCode : $event.which;
      if ((keyCode < 48 || keyCode > 57) && keyCode !== 46) {
        // 46 is dot 189 and 109 is -
        $event.preventDefault();
      }
    },
    async handleStationUpdate() {
      try {
        if (this.editation) {
          await stationApi.updateStation(this.entity);
        } else {
          await stationApi.createStation({
            name: this.entity.name,
            latitude: Number(this.entity.latitude),
            longitude: Number(this.entity.longitude),
          });
        }
        this.modal = undefined;
        this.$notify({
          group: "global",
          title: this.$i18n.tc(`notifications.station.${this.editation ? "update" : "created"}`),
          type: "success",
        });
        this.editation = false;
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("settings.somethingWrong"),
          type: "error",
        });
        console.error(e);
      }
      await this.getAllStations();
    },
    async handleRouteUpdate() {
      const entity = cloneDeep(this.entity);
      const promise = this.editation ? routeApi.updateRoute(entity) : routeApi.addRoute(entity);
      try {
        await promise;
        this.modal = undefined;
        this.$notify({
          group: "global",
          title: this.$i18n.tc(`notifications.route.${this.editation ? "update" : "created"}`),
          type: "success",
        });
        this.editation = false;
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("settings.somethingWrong"),
          type: "error",
        });
        console.error(e);
      }
      await this.getAllRoutes();
    },
    async handleCarUpdate() {
      const entity = cloneDeep(this.entity);
      const promise = this.editation ? carApi.updateCar(entity) : carApi.addCar(entity);
      try {
        await promise;
        this.modal = undefined;
        this.$notify({
          group: "global",
          title: this.$i18n.tc(`notifications.car.${this.editation ? "update" : "create"}`),
          type: "success",
        });
        this.editation = false;
      } catch (e) {
        this.$notify({
          group: "global",
          title: this.$i18n.tc("settings.somethingWrong"),
          type: "error",
          text: e,
        });
      }
      await this.getAllCars();
    },
  },
};
</script>

<style lang="scss" scoped>
.color-circle {
  width: 20px;
  height: 20px;
  border-radius: 100%;
  display: block;
}
</style>
