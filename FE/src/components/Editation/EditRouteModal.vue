<template>
  <v-form>
    <v-container>
      <v-row>
        <v-col cols="12" md="6">
          <ValidationProvider v-slot="{ errors }" :name="$t('general.name')" rules="required">
            <v-text-field
              :error-messages="errors"
              :label="$t('general.name')"
              :value="route.name"
              hide-details
              required
              @input="$emit('update:route', { ...route, name: $event })"
            />
          </ValidationProvider>
        </v-col>

        <v-col cols="12" md="6">
          <v-text-field
            v-model="color"
            :label="$t('routes.color')"
            :value="route.color"
            hide-details
          />
        </v-col>

        <v-col cols="12" md="12">
          <v-color-picker v-model="color" hide-inputs hide-mode-switch />
        </v-col>

        <v-col cols="12" md="12">
          <v-row v-for="(stop, index) in route.stops" :key="index" align="center">
            <v-col cols="12" md="6">
              <v-text-field
                :label="$t('stations.position')"
                :value="positionValue(stop)"
                hide-details
                @input="handleChangeStopVal(index, getLatLong($event))"
              >
                <template #append>
                  <v-tooltip top>
                    <template #activator="{ on }">
                      <v-icon x-small v-on="on"> mdi-help-circle-outline</v-icon>
                    </template>
                    {{ $t("routes.order") }}: {{ stop.order }}
                  </v-tooltip>
                </template>
              </v-text-field>
            </v-col>
            <v-col class="d-flex align-center" cols="12" md="6">
              <v-select
                :items="stations"
                :label="$t('settings.stations')"
                :value="stop.station && stop.station.id ? stop.station.id : undefined"
                clearable
                hide-details
                item-text="name"
                item-value="id"
                return-object
                @input="handleChangeStopVal(index, getStation($event))"
              />
              <v-btn
                :block="$vuetify.breakpoint.mobile"
                :icon="!$vuetify.breakpoint.mobile"
                color="error"
                small
                @click="handleRemovePoint(index)"
              >
                <v-icon small> mdi-delete</v-icon>
              </v-btn>
            </v-col>
          </v-row>
          <v-row align="center" justify="center">
            <v-col cols="12" md="2">
              <v-btn
                :block="$vuetify.breakpoint.mobile"
                :icon="!$vuetify.breakpoint.mobile"
                color="success"
                @click="handleAddPoint()"
              >
                <v-icon> mdi-plus-circle-outline</v-icon>
              </v-btn>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>

<script>
import { ValidationProvider } from "vee-validate";
import { getLatLong, getPositionValue } from "../../code/helpers/positionHelpers";
import { getStation } from "../../code/helpers/routesHelpers";

export default {
  name: "EditRouteModal",
  components: { ValidationProvider },
  props: {
    route: {
      type: Object,
      required: true,
    },
    stations: {
      type: Array,
      default: () => {
        return [];
      },
    },
  },
  computed: {
    color: {
      get() {
        return this.route.color;
      },
      set(val) {
        this.$emit("update:route", { ...this.route, color: val });
      },
    },
  },
  methods: {
    getStation,
    getLatLong,
    positionValue(stop) {
      return getPositionValue(stop);
    },
    handleRemovePoint(index) {
      this.route.stops.splice(index, 1);
    },
    handleChangeStopVal(index, val) {
      const { stops } = this.route;
      stops[index] = { ...stops[index], ...val };
      this.$emit("update:route", { ...this.route, stops });
    },
    handleAddPoint() {
      const { stops } = this.route;
      const max = stops.reduce((prev, current) => {
        return prev > current.order ? prev : current.order;
      }, 0);
      stops.push({ longitude: 0, latitude: 0, order: max + 1, station: { id: undefined } });
      this.$emit("update:route", { ...this.route, stops });
    },
  },
};
</script>
