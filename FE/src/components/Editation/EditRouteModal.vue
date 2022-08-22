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
              required
              @input="$emit('update:route', { ...route, name: $event })"
            />
          </ValidationProvider>
        </v-col>
        <v-col cols="12" md="6">
          <v-text-field
            v-model="color"
            :background-color="color"
            :value="route.color"
            append-icon="mdi-invert-colors"
            hide-details
            outlined
            @click:append="hover = !hover"
          />
        </v-col>
        <v-col cols="12" md="12">
          <v-row>
            <v-col cols="12" md="6">
              <v-color-picker
                v-if="hover"
                v-model="color"
                elevation="0"
                hide-inputs
                hide-mode-switch
              />
              <v-col cols="6" md="8">
                <v-btn justify="center" @click="isHidden = !isHidden">
                  {{ $t("routes.coordinates") }}
                </v-btn>
              </v-col>
            </v-col>
            <v-col cols="12" md="6">
              <v-card class="overflow-y-auto overflow-x-hidden" max-height="370">
                <v-row v-for="(stop, index) in route.stops" :key="index" align="center">
                  <v-col
                    :color="color"
                    class="d-flex align-center"
                    cols="12"
                    data-test="index"
                    md="12"
                  >
                    <v-select
                      :items="stations"
                      :label="$t('settings.stations')"
                      :value="stop.station && stop.station.id ? stop.station.id : undefined"
                      clearable
                      hide-details
                      item-text="name"
                      item-value="id"
                      return-object
                      solo
                      @input="handleChangeStationVal(index, getStation($event))"
                      @click:clear="handleRemovePointStation(index, getStation($event))"
                    />
                  </v-col>
                </v-row>
                <v-row align="center" justify="center">
                  <v-btn
                    :icon="!$vuetify.breakpoint.mobile"
                    color="success"
                    @click="handleAddPoint()"
                  >
                    <v-icon> mdi-plus-circle-outline</v-icon>
                  </v-btn>
                </v-row>
              </v-card>
            </v-col>
          </v-row>
        </v-col>
        <v-col v-if="!isHidden" cols="12" md="12">
          <v-card class="overflow-y-auto" max-height="200">
            <v-responsive :aspect-ratio="16 / 9">
              <v-card-text>
                <v-row v-for="(stop, index) in route.stops" :key="index" align="center">
                  <v-col cols="12" md="11">
                    <v-text-field
                      :label="$t('stations.position')"
                      :value="positionValue(stop)"
                      hide-details
                      @input="handleChangeStopVal(index, getLatLong($event))"
                      @keypress="onlyNumber"
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
                  <v-btn
                    :icon="!$vuetify.breakpoint.mobile"
                    color="error"
                    small
                    @click="handleRemovePoint(index)"
                  >
                    <v-icon small> mdi-delete</v-icon>
                  </v-btn>
                </v-row>
                <v-row align="center" justify="center">
                  <v-btn
                    :icon="!$vuetify.breakpoint.mobile"
                    color="success"
                    @click="handleAddPoint()"
                  >
                    <v-icon> mdi-plus-circle-outline</v-icon>
                  </v-btn>
                </v-row>
              </v-card-text>
            </v-responsive>
          </v-card>
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
  data() {
    return {
      isHidden: true,
      hover: true,
    };
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
    onlyNumber($event) {
      // console.log($event.keyCode); //keyCodes value
      const keyCode = $event.keyCode ? $event.keyCode : $event.which;
      if ((keyCode < 48 || keyCode > 57) && keyCode !== 46) {
        // 46 is dot
        $event.preventDefault();
      }
    },
    handleRemovePointStation(index) {
      const orderX = this.route.stops[index].latitude;
      const orderY = this.route.stops[index].longitude;
      const orderObj = this.route.stops[index].order;
      this.route.stops[index] = {
        latitude: orderX,
        longitude: orderY,
        order: orderObj,
        station: null,
      };
    },
    handleChangeStopVal(index, val) {
      const { stops } = this.route;
      stops[index] = { ...stops[index], ...val };
      this.$emit("update:route", { ...this.route, stops });
    },
    handleChangeStationVal(index, val) {
      const { stops } = this.route;
      stops[index] = { ...stops[index], ...val };
      console.log("VAL", val, index);
      this.$emit("update:route", { ...this.route, stops });
    },
    handleAddPoint() {
      const { stops } = this.route;
      const max = stops.reduce((prev, current) => {
        return prev > current.order ? prev : current.order;
      }, 0);
      stops.push({ longitude: 0, latitude: 0, order: max + 1, station: { id: this.nextId } });
      this.nextId += 1;
      this.$emit("update:route", { ...this.route, stops });
    },
  },
};
</script>
